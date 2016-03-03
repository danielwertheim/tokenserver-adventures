(function ($, swaggerUi) {
    $(function () {
        var settings = {
            authority: 'https://localhost:44300/identity',
            client_id: 'swashy',
            popup_redirect_uri: window.location.protocol
                + '//'
                + window.location.host
                + '/tokenclient/popup.html',

            response_type: 'id_token token',
            scope: 'openid profile SecuredApi',

            filter_protocol_claims: true
        },
        manager = new OidcTokenManager(settings),
        $inputApiKey = $('#input_apiKey');

        //$inputApiKey.on('change', function () {
        //    var key = encodeURIComponent($('#input_apiKey')[0].value);
        //    if (key && key.trim() !== '') {
        //        var apiKeyAuth = new SwaggerClient.ApiKeyAuthorization(
        //            'Authorization',
        //            'Bearer ' + key,
        //            'header');
        //        swaggerUi.api.clientAuthorizations.add('api_key', apiKeyAuth);
        //    }
        //});

        $inputApiKey.on('dblclick', function () {
            manager.openPopupForTokenAsync()
                .then(function () {
                    $inputApiKey.val(manager.access_token).change();
                }, function (error) {
                    console.error(error);
                });
        });
    });
})(jQuery, window.swaggerUi);