using Esercizio_Gestione_Albergo.DataAccess;
using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.Services.DAO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Esercizio_Gestione_Albergo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DettagliController : ControllerBase
    {
        private readonly IDettaglioSoggiornoDAO _dettagliSoggiornoDAO;
        private readonly IServizioAggiuntivoDAO _servizioAggiuntivoDAO;
        private readonly ITipologiaCameraDAO _tipologiaCameraDAO;
        private readonly IClienteDAO _clienteDAO;

        public DettagliController(
            IDettaglioSoggiornoDAO dettagliSoggiornoDAO,
            IServizioAggiuntivoDAO servizioAggiuntivoDAO,
            IClienteDAO clienteDAO,
            ITipologiaCameraDAO tipologiaCameraDAO)
        {
            _dettagliSoggiornoDAO = dettagliSoggiornoDAO;
            _servizioAggiuntivoDAO = servizioAggiuntivoDAO;
            _tipologiaCameraDAO = tipologiaCameraDAO;
            _clienteDAO = clienteDAO;
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
        public ActionResult<IEnumerable<Cliente>> GetClienti()
        {
            var clienti = _clienteDAO.GetAll();
            return Ok(clienti);
        }
    }
}
