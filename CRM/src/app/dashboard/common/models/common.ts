import { AuthDetails } from "src/app/shared/models/authDetails"

export interface LastValue {
    lastValue: string
}

export interface CheckForPermission {
    authDto: AuthDetails
    location: string
    event: string
}

export interface CheckForPermissionReponse {
    hasAccess: string
}