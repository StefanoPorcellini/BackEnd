$(document).ready(function () {
    populateSelect('/api/Dettagli/clienti', '#ClienteCodiceFiscale');
    populateSelect('/api/Dettagli/dettaglio-soggiorno', '#DettaglioSoggiorno');
    populateSelect('/api/Dettagli/servizi-aggiuntivi', '#ServiziAggiuntivi');
    populateSelect('/api/Dettagli/tipologie-camere', '#TipologiaCamera');

    // Funzione per popolare il select con i dati dal server
    function populateSelect(url, selectId) {
        $.ajax({
            url: url,
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                var select = $(selectId);
                select.empty(); // Pulisce le precedenti opzioni
                select.append('<option value="">Seleziona...</option>');
                $.each(data, function (index, item) {
                    select.append($('<option>', {
                        value: item.id || item.codiceFiscale, // Usa l'ID o il codice fiscale per il valore dell'opzione
                        text: item.descrizione || item.nome // Usa la descrizione o il nome per il testo dell'opzione
                    }));
                });
            },
            error: function () {
                alert('Errore durante il caricamento dei dati.');
            }
        });
    }
});