export interface notification {
    id: number;
    notify: number;
    date: Date;
    time: string;
    message: string;
    priorityid: number;
    forwardto: string;
    snoozeon: string;
    addby: string;
    addon: Date;
    from: Date;
}

export interface vNofitication {
    Id: number
    Name: string
    Date: string
    Time: string
    Message: string
    prority: number
    AddBy: string
    AddOn: string
    ForwardTo: string
    SnoozeOn: string
    From: string
}