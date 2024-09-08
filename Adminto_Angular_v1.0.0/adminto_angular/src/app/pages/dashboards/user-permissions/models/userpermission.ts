import { AuthDetails } from "src/app/shared/models/methods"

export interface UserPermissionBasedOnUser {
    accessLocation: string
    event: Event[]
}

export interface Event {
    value: string
    hasPermission: string
}

export interface UserInfoForCombo {
    username: string
    userId: number
}

export interface UpdateUserPermissionRequest {
    authDto: AuthDetails
    accessLocation: string
    event: string
    hasPermission: string
    userId: number
}

export interface MakeRequestForUserPermission {
    authDto: AuthDetails
    userId: string
    hasPermission: string
}

export interface staff{
    id :number;
    name :string;
    designation :string;
    mobileno :string;
    parentid :string;
    addby :string;
    addon :Date;
    status :number;
    userid :number;
    
    firstname : string;
    lastname : string;
    email : string;
    passport : string;
    userimage : string;
    visaIssueDate : Date;
}