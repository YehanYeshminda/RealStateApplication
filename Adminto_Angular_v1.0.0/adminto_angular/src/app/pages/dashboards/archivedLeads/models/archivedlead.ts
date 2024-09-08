import { AuthDetails } from "src/app/shared/models/methods"

export interface ArchivedLeadsAll {
    data: ArchivedLeadList[]
    totalCountPages: number
    totalData: number
  }
  
  export interface ArchivedLeadList {
    leadNo: string
    name: string
    phone: string
    email: string
    otherNo: string
    receivedOn: string
    assignOn: string
    called: number
    status: number
    source: string
    staffName: string
    contactMethod: string
    leadStatus: string
    campaignId: string
    comments: string
    importance: number
  }

export interface ArchivedLeadListDtoRequest {
  authDto: AuthDetails
  pageSize: number
  page: number
}