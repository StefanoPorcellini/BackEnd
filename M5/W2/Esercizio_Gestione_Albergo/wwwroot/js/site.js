$(document).ready(function () {
    populateSelect('/api/Api/clienti', '#ClienteCodiceFiscale', 'codiceFiscale');
    populateSelect('/api/Api/dettaglio-soggiorno', '#DettaglioSoggiorno');
    populateSelect('/api/Api/servizi-aggiuntivi', '#ServiziAggiuntivi');
    populateSelect('/api/Api/tipologie-camere', '#TipologiaCamera');
    populateSelectCamere('/api/Api/camere', '#CameraNumero', 'numero'); // Aggiunta per le camere

    // Funzione per popolare il select con i dati dal server
    function populateSelect(url, selectId, valueField = 'id') {
        $.ajax({
            url: url,
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                var select = $(selectId);
                select.empty(); // Pulisce le precedenti opzioni
                select.append('<option value="">Seleziona...</option>');
                $.each(data, function (index, item) {
                    var optionText = item.descrizione || (item.nome + ' ' + item.cognome); // Usa descrizione, nome completo per il testo dell'opzione
                    select.append($('<option>', {
                        value: item[valueField], // Usa il campo specificato per il valore dell'opzione
                        text: optionText
                    }));
                });
            },
            error: function () {
                alert('Errore durante il caricamento dei dati.');
            }
        });
    }

    function populateSelectCamere(url, selectId, valueField = 'id') {
        $.ajax({
            url: url,
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                var select = $(selectId);
                select.empty(); // Pulisce le precedenti opzioni
                select.append('<option value="">Seleziona...</option>');
                $.each(data, function (index, item) {
                    var optionText = ('Camera n°' + item.numero); // Usa stringa personalizzata per il testo dell'opzione
                    select.append($('<option>', {
                        value: item[valueField], // Usa il campo specificato per il valore dell'opzione
                        text: optionText
                    }));
                });
            },
            error: function () {
                alert('Errore durante il caricamento dei dati.');
            }
        });
    }

    $('#Dal, #Al, #CaparraConfirmatoria, #CameraNumero, #DettaglioSoggiorno').on('change', function () {
        calculateTariffa();
    });

    function calculateTariffa() {
        var dal = new Date($('#Dal').val());
        var al = new Date($('#Al').val());
        var caparra = parseFloat($('#CaparraConfirmatoria').val()) || 0;
        var giorni = Math.max((al - dal) / (1000 * 60 * 60 * 24) + 1, 0);

        var cameraNumero = $('#CameraNumero').val();
        var dettaglioSoggiornoId = $('#DettaglioSoggiorno').val();

        if (giorni <= 0) {
            $('#Tariffa').val('');
            $('#SaldoFinale').val('');
            return;
        }

        if (cameraNumero && dettaglioSoggiornoId) {
            $.ajax({
                url: `/api/Api/tariffa-calcolo?cameraNumero=${cameraNumero}&dettaglioSoggiornoId=${dettaglioSoggiornoId}&giorni=${giorni}`,
                method: 'GET',
                dataType: 'json',
                success: function (data) {
                    if (data && data.tariffa !== undefined) {
                        $('#Tariffa').val(data.tariffa.toFixed(2));
                        $('#SaldoFinale').val((data.tariffa - caparra).toFixed(2));
                    } else {
                        $('#Tariffa').val('');
                        $('#SaldoFinale').val('');
                    }
                },
                error: function () {
                    alert('Errore durante il calcolo della tariffa.');
                    $('#Tariffa').val('');
                    $('#SaldoFinale').val('');
                }
            });
        } else {
            $('#Tariffa').val('');
            $('#SaldoFinale').val('');
        }
    }

    function numeroProgressivoPrenotazione() {
        $.ajax({
            url: `/api/Api/prenotazioni`,
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.length !== 0) {
                    var numeroProgressivo = 'PR' + (data.length + 1);
                    $('#NumeroProgressivo').val(numeroProgressivo);
                }
            },
            error: function () {
                alert('Errore durante il caricamento delle prenotazioni.');
            }
        });
    }

    // Chiamata alla funzione per generare NumeroProgressivo
    numeroProgressivoPrenotazione();
    // Chiamata alla funzione per calcolare la tariffa
    calculateTariffa();
});
