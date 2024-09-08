export interface advPayment{
    id : number;
    date : Date;
    salesby : number;
    customer : number;
    address : string;
    chequepaid : number;
    chequeno : string;
    cashpaid : number;
    cardpaid : number;
    cardbank : string;
    paymentfor : string;
    description : string;
}


export interface VadvPayment{
    id : number;
    date : Date;
    typeName : string;
    custName : string;
    address : string;
    chequepaid : number;
    chequeno : string;
    cashpaid : number;
    cardpaid : number;
    bankCode : string;
    propertname : string;
    description : string;
}


