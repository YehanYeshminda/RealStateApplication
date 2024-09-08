export interface calllist{
    id : number ;
    firstName : string ;
    lastName : string ;
    email : string ;
    phoneNo : string ;
    phoneNo2 : string ;
}

export interface CallListPaginationData {
    data: calllist[]
    totalCountPages: number
    totalData: number
  }

