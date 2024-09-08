import { AuthDetails } from "src/app/shared/models/authDetails"

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