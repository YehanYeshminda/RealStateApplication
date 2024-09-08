export interface agree{
    id : number;
    date : Date;
    custcode : number;
    agreementtype : string;
    enddate : Date;
    remindon : Date;
    remarks : string;
    status : number;
}

export interface Vagree{
    id : number;
    date : Date;
    custName : string;
    typeName : string;
    enddate : Date;
    remindon : Date;
    remarks : string;
    status : number;
}