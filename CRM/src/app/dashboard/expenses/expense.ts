export interface expense{
    id : string ;
    vDate : Date ;
    supplierId : string ;
    mainCatId : string ;
    subcatId : string ;
    description : string ;
    cashPaid : number ;
    chequePaid : number ;
    chequeNo : string ;
    status : number ;
    authBy : string ;
    receiptNo : string ;
    accountId : number ;
    userId : number ;
    rDate : Date ;
    uniqueId : string ;
    totalValue : number ;
    brid : number;
    vatp : number ;
    vat : number ;
    netTotal : number ;
    paid : number ;
}


export interface VExpense {
    id: number;
    mainCatId: string;
    mainCatergory: string;
    subCatId: string;
    subCatergory: string;
    status: number;
  }
  

export interface expenseview{
    id : string;
    vDate : Date;
    supplierName : string;
    accountId : string;
    mainCategory : string;
    subCategory : string;
    description : string;
    cashPaid : number;
    chequePaid : number;
    chequeNo : string;
    cid : string;
    status : string;
    username : string;
    receiptNo : string;
    rDate : Date;
    branchName : string;
    totalValue : number;
    vatp : number;
    vat : number;
    netTotal : number;
    paid : number;
}


