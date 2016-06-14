class BcInvoiceController extends baseController {
    public userService: UserService;
    public invoices;

    constructor() {
        super();
        this.userService = new UserService();
    }

    init(): void {
        baseController.userAuthValidate(() => {
            this.getAllInvoice();
            this.regTabClickEvent();
        });
    }
    

    updateInvoice(id): void {
        var invoice = this.getInvoiceById(id);
        invoice.isVat = $('#isVat').val();
        invoice.isVat = $('#isVat').val();
        invoice.isCompany = $('#isCompany').val();
        invoice.companyName = $('#companyName').val();
        invoice.vatCompanyName = $('#vatCompanyName').val();
        invoice.vatCode = $('#vatCode').val();
        invoice.vatAddress = $('#vatAddress').val();
        invoice.vatPhone = $('#vatPhone').val();
        invoice.vatBankName = $('#vatBankName').val();
        invoice.vatBankAccount = $('#vatBankAccount').val();
        invoice.consigneeName = $('#consigneeName').val();
        invoice.consigneeMobile = $('#consigneeMobile').val();
        invoice.consigneeAddress = $('#consigneeAddress').val();

        if (id == 0) {
            this.userService.addUserInvoice(invoice,(json) => {
                if (json.isSuccess) {
                    Utils.tipAlert(true, "添加成功!");
                    this.getAllInvoice();
                }
            });
        } else {
            this.userService.updateUserInvoice(invoice,(json) => {
                if (json.isSuccess) {
                    Utils.tipAlert(true, "更新成功!");
                    this.getAllInvoice();
                }
            });
        }
    }

    regCheckBoxClickEvent(): void {
        $('input[name=invoice]').on('change',() => {
            var invoice = this.getInvoiceById($('input[name=invoice]:checked').val());
            this.loadInput(invoice);
        });
    }


    delInvoice(id): void {
        this.userService.delUserInvoice(Number(id),(json) => {
            if (json.isSuccess) {
                Utils.tipAlert(true, "已删除!");
                this.getAllInvoice();
            }
        });
    }

    getInvoiceById(id: number): any {
        var invoice;
        for (var i = 0; i < this.invoices.length; i++) {
            if (this.invoices[i].id == id) {
                invoice = this.invoices[i];
                break;
            }
        }
        if (!invoice) {
            invoice = {
                "groupSN": "",
                "userName": "",
                "isOrder": false,
                "isDefault": false,
                "isVat": false,
                "isCompany": false,
                "companyName": "",
                "vatCompanyName": "",
                "vatCode": "",
                "vatAddress": "",
                "vatPhone": "",
                "vatBankName": "",
                "vatBankAccount": "",
                "consigneeName": "",
                "consigneeMobile": "",
                "consigneeAddress": "",
                "id": id
            };
        }
        return invoice;
    }

    loadInput(invoice): void {
        if (!invoice)
            return;
        $('#vatCompanyName').val(invoice.vatCompanyName);
        $('#vatCode').val(invoice.vatCode);
        $('#vatAddress').val(invoice.vatAddress);
        $('#vatPhone').val(invoice.vatPhone);
        $('#vatBankName').val(invoice.vatBankName);
        $('#vatBankAccount').val(invoice.vatBankAccount);
        $('#companyName').val(invoice.companyName);
        $('#consigneeName').val(invoice.consigneeName);
        $('#consigneeMobile').val(invoice.consigneeMobile);
        $('#consigneeAddress').val(invoice.consigneeAddress);
        $('#isVat').val(invoice.isVat);
        $('#isCompany').val(invoice.isCompany);


        if (invoice.isVat) {
            $('.sc3_change .sc3_chgBnt').eq(1).click();
        }
        else {
            $('.sc3_change .sc3_chgBnt').eq(0).click();
            if (invoice.isCompany) {
                $('.sc3_f1 .sc3_chgBnt').eq(1).click();
            } else {
                $('.sc3_f1 .sc3_chgBnt').eq(0).click();
            }
        }
    }

    getAllInvoice(): void {
        this.userService.getUserInvoices((json) => {
            if (json.isSuccess) {
                this.invoices = json.invoices;
                $("#invoicelist").html("");

                var innerHtml = '';

                for (var i = 0; i < json.invoices.length; i++) {

                    var invoiceName = '';
                    if (json.invoices[i].isVat) {
                        invoiceName = "增值税发票 " + json.invoices[i].vatCompanyName;
                    } else {
                        invoiceName = "普通发票 " + (json.invoices[i].isCompany ? json.invoices[i].companyName : "个人");
                    }
                    innerHtml += '<li>';
                    innerHtml += '  <input type="radio" value="' + json.invoices[i].id + '" id = "invoice' + json.invoices[i].id + '" ' + (i == 0 ? 'checked' : '') + ' name="invoice" class="bmra_rad"/>';
                    innerHtml += '  <label for="invoice' + json.invoices[i].id + '"> ' + invoiceName + ' </label>';
                    innerHtml += '  <a href="javascript:;" onclick="new BcInvoiceController().delInvoice(' + json.invoices[i].id + ');" class="btndel cor_red">删除</a >';
                    innerHtml += '</li>';
                }

                innerHtml += '<li><input type="radio"  id="invoicenew" name="invoice" class="bmra_rad"><label for= "invoicenew"> 使用新的发票信息 </label></li>';

                $("#invoicelist").html(innerHtml);
                this.loadInput(json.invoice);
                this.regCheckBoxClickEvent();
            }
        });
    }

    regTabClickEvent(): void {
        $('.sc3_change .sc3_chgBnt').on('click', function () {
            $('.sc3_change .sc3_chgBnt').removeClass('sc3_cutChgBnt');
            $(this).addClass('sc3_cutChgBnt');

            $('.ibox').hide().eq(1 - $(".sc3_change .sc3_chgBnt").index(this)).show();

            if ($(".sc3_change .sc3_chgBnt").index(this) == 1) {
                $('#isVat').val('true');
            } else {
                $('#isVat').val('false');
            }
        });

        $('.sc3_f1 .sc3_chgBnt').on('click', function () {
            $('.sc3_f1 .sc3_chgBnt').removeClass('sc3_cutChgBnt');
            $(this).addClass('sc3_cutChgBnt');
            if ($(this).text() == '单位') {
                $('#companyName').show();
                $('#isCompany').val('true');
            } else {
                $('#companyName').hide();
                $('#isCompany').val('false');
            }
        });

        $('.sc3mr_submit').on('click',() => {
            var checkedVal = $('input[name=invoice]:checked').val()
            if (Number(checkedVal)) {
                this.updateInvoice(Number(checkedVal));
            } else {
                this.updateInvoice(0);
            }
        });
    }

    

}


