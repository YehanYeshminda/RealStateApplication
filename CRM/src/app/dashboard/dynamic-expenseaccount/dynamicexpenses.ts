import { AuthDetails } from "src/app/shared/models/authDetails"


export interface SendDynamicexpense {
    authDto: AuthDetails
    dynamicField: string
    mainCatId: number
    subCatId: string
    status: number
}

export interface GetDynamicexpense {
    dynamicField: string
    mainCatId: number
    subCatId: string
    status: number
}