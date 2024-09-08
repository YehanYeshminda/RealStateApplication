import { AuthDetails } from "src/app/shared/models/authDetails"


export interface SendDynamicFormRequest {
    authDto: AuthDetails
    dynamicField: string
    catergoryName: string
    remark: string
    status: number
}

export interface GetDynamicFormRequest {
    categoryId: number
    itemCategory: string
    remark: string
    status: number
    cid: any
}