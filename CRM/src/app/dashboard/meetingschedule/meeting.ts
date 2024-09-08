export interface meetsched{
    id : number;
    name : string;
    date : Date;
    staffid : number;
    reason : string;
    custid : number;
    meetdate : Date;
    meettime : string;
    venue : string;
    remarks : string;
    status : number;
    conclusion : string;

    staffIds: number[]
}


export interface updatemeet{
    id : number;
    name : string;
    conclusion : string;
}

export interface remeet{
    id : number;
    meetdate : Date;
    meettime : string;
    venue : string;
}

export interface vmeeting{
    id : number;
    name : string;
    date : Date;
    expr1 : string;
    reason : string;
    custName : string;
    meetdate : Date;
    meettime : string;
    venue : string;
    remarks : string;
    status : number;
    conclusion : string;
}