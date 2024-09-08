
  export interface MessageResponsePaginationResult {
    data: MessageResponse[]
    totalCountPages: number
    totalData: number
  }
  
  export interface MessageResponse {
    id: number
    fromFirstName: string
    fromLastName: string
    dateAdded: string
    time: string
    message: string
    priorityId: string
    addBy: string
    addonTime: string
    forwardTo: string
    snoozeOn: string
    fromTime: string
    
  }

  export interface notificationdto {
     id : number
     notify : number
     from : Date 
     date : Date 
     time :  string 
     message :  string 
     priorityid : number
     forwardto :  string 
     snoozeon : Date 
  }