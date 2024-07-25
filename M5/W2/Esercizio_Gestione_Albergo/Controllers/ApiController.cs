using Esercizio_Gestione_Albergo.DataAccess;
using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.Services.DAO;
using Esercizio_Gestione_Albergo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Esercizio_Gestione_Albergo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IDettaglioSoggiornoDAO _dettagliSoggiornoDAO;
        private readonly IServizioAggiuntivoDAO _servizioAggiuntivoDAO;
        private readonly ITipologiaCameraDAO _tipologiaCameraDAO;
        private readonly ICameraDAO _cameraDAO;
        private readonly IClienteDAO _clienteDAO;
        private readonly IPrenotazioneDAO _prenotazioneDAO;

        public ApiController(
            IDettaglioSoggiornoDAO dettagliSoggiornoDAO,
            IServizioAggiuntivoDAO servizioAggiuntivoDAO,
            IClienteDAO clienteDAO,
            ICameraDAO cameraDAO,
            IPrenotazioneDAO prenotazioneDAO,
            ITipologiaCameraDAO tipologiaCameraDAO)
        {
            _dettagliSoggiornoDAO = dettagliSoggiornoDAO;
            _servizioAggiuntivoDAO = servizioAggiuntivoDAO;
            _tipologiaCameraDAO = tipologiaCameraDAO;
            _clienteDAO = clienteDAO;
            _cameraDAO = cameraDAO;
            _prenotazioneDAO = prenotazioneDAO;
        }

        [HttpGet("dettaglio-soggiorno")]
        public ActionResult<IEnumerable<DettagliSoggiorno>> GetDettaglioSoggiorno()
        {
            var dettagliSoggiorno = _dettagliSoggiornoDAO.GetAll();
            return Ok(dettagliSoggiorno);
        }

        [HttpGet("servizi-aggiuntivi")]
        public ActionResult<IEnumerable<ServizioAggiuntivo>> GetServiziAggiuntivi()
        {
            var serviziAggiuntivi = _servizioAggiuntivoDAO.GetAll();
            return Ok(serviziAggiuntivi);
        }

        [HttpGet("tipologie-camere")]
        public ActionResult<IEnumerable<TipologiaCamera>> GetTipologieCamere()
        {
            var tipologieCamere = _tipologiaCameraDAO.GetAll();
            return Ok(tipologieCamere);
        }

        [HttpGet("clienti")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClienti()
        {
            var clienti = await _clienteDAO.GetAll();
            return Ok(clienti);
        }

        [HttpGet("camere")]
        public async Task<ActionResult<IEnumerable<CameraViewModel>>> GetAll()
        {
            var camere = await _cameraDAO.GetCamere();
            return Ok(camere);
        }

        [HttpGet("tariffa-calcolo")]
        public async Task<IActionResult> CalcolaTariffa(int cameraNumero, int dettaglioSoggiornoId, int giorni)
        {
            try
            {
                var tariffa = await _prenotazioneDAO.CalcoloTariffaAsync(cameraNumero, dettaglioSoggiornoId, giorni);
                return Ok(new { tariffa });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Errore durante il calcolo della tariffa.", error = ex.Message });
            }
        }

    }
}
