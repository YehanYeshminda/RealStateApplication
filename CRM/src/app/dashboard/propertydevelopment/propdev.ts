export interface propdev {
    id: string;
    date: Date;
    propname: string;
    vender: number;
    propertyno: string;
    expenseaccount: number;
    description: string;
    amount: number;
    cashpaid: number;
    banktransfer: number;
    bankid: number;
    chequepaid: number;
    chequeid: number;
    approvedby: number;
}

export interface PropertyDevView {
    Id: string
    Date: string
    PropertyName: string
    SupplierName: string
    Propertyno: string
    Expenseaccount: number
    Description: string
    Amount: number
    Cashpaid: number
    Banktransfer: number
    BankCode: string
    Chequepaid: number
    ChequeId: string
    ApprovedBy: string
    Addon: string
    AddBy: string
}