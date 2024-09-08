import { AuthDetails } from "src/app/shared/models/authDetails"

export interface PaymentSchedule {
    authDto: AuthDetails
    PaymentScheduleNo: string
    Date: string
    SupplierName: string
    reason: string
    rxpaccount: string
    amount: number
    paidon: string
    renewevery: string
    renewstatus: string
    status: number
}

export interface VExpenseAccountData {
    ID: number
    MainCatID: string
    MainCategory: string
    SubCatID: string
    SubCategory: string
    Status: number
}


// export interface PaymentScheduleList {
//     id: number
//     paymentScheduleNo: string
//     date: string
//     venderid: number
//     reason: string
//     rxpaccount: string
//     amount: number
//     paidon: string
//     renewevery: string
//     renewstatus: string
//     status: number
// }

export interface PaymentScheduleList {
    PaymentScheduleNo: string
    Date: string
    SupplierName: string
    reason: string
    rxpaccount: string
    amount: number
    paidon: string
    renewevery: string
    renewstatus: string
    status: number
}