$(function () {

    setModal();
    buscaCep();

    $("#imageUpload").change(function () {

        $("#image_name").val(this.files[0].name);
        if ($("#image_name").val() != '') {
            $("#error-message").hide();
        }
    });

    $("#btnCreate").on("click", function () {

        if (!$("#image_name").val()) {
            $("#error-message").text("Preencha o campo Image").show();
        } else {
            $("#error-message").hide();
        }
    });

    // Inicializa o Toastr
    toastr.options = {
        positionClass: "toast-top-right" // Define a posição das notificações
    };
});

function setModal() {
            $.ajaxSetup({ cache: false });
    $(document).on("click", "a[data-modal]", function (e) {
        $('#myModalContent').load(this.href, function () {
            $('#myModal').modal('show', {
                keyboard: true
            });
            bindForm(this);
        });
        return false;
    });
}

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    $('#AddressTarget').load(result.url); // Carrega o resultado HTML para a div demarcada
                } else {
                    $('#myModalContent').html(result);
                    bindForm(dialog);
                }
            }
        });
        setModal();
        return false;
    });
}

function buscaCep() {


        function limpa_formulário_cep() {
            // Limpa valores do formulário de cep.
            $("#address_StreetName").val("");
            $("#address_Neighborhood").val("");
            $("#address_City").val("");
            $("#address_State").val("");
        }

        //Quando o campo cep perde o foco.
        $("#address_ZipCode").blur(function () {

            //Nova variável "cep" somente com dígitos.
            var cep = $(this).val().replace(/\D/g, '');

            //Verifica se campo cep possui valor informado.
            if (cep != "") {

                //Expressão regular para validar o CEP.
                var validacep = /^[0-9]{8}$/;

                //Valida o formato do CEP.
                if (validacep.test(cep)) {

                    //Preenche os campos com "..." enquanto consulta webservice.
                    $("#address_StreetName").val("...");
                    $("#address_Neighborhood").val("...");
                    $("#address_City").val("...");
                    $("#address_State").val("...");

                    //Consulta o webservice viacep.com.br/
                    $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?",
                        function (dados) {

                            if (!("erro" in dados)) {
                                //Atualiza os campos com os valores da consulta.
                                $("#address_StreetName").val(dados.logradouro);
                                $("#address_Neighborhood").val(dados.bairro);
                                $("#address_City").val(dados.localidade);
                                $("#address_State").val(dados.uf);
                            } //end if.
                            else {
                                //CEP pesquisado não foi encontrado.
                                limpa_formulário_cep();
                                alert("CEP não encontrado.");
                            }
                        });
                } //end if.
                else {
                    //cep é inválido.
                    limpa_formulário_cep();
                    alert("Formato de CEP inválido.");
                }
            } //end if.
            else {
                //cep sem valor, limpa formulário.
                limpa_formulário_cep();
            }
        });
}


