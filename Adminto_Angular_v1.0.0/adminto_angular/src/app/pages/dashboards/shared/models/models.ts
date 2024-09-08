import { AuthDetails } from "src/app/shared/models/methods"

export interface VExpenseAccountData {
    ID: number
    MainCatID: string
    MainCategory: string
    SubCatID: string
    SubCategory: string
    Status: number
}

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

export interface DynamicTable {
    id: number;
    dynamicField: string;
    remark: string;
    status: number;
    cId: number;
  }
  
  export interface DynamicExpenseTable {
    id: number;
    mainCategory: string;
    subCategory: string;
    status: number;
  }

  export interface CheckForPermission {
    authDto: AuthDetails
    location: string
    event: string
}

export interface CheckForPermissionReponse {
    hasAccess: string
}

export interface ComboInfo {
    value: number;
    textValue: string;
}

export interface ComboInfoBank {
    value: string;
    textValue: string;
}

export interface CallCenterInfo {
    source: string
    leadName: string
    receivedOn: string
    leadStatus: number
}

export interface CallCenterRequest {
    authDto: AuthDetails
    leadStatus: number
    leadNo: string
    remark: string
}

export interface LeadLogResponseByLeadNo {
    id: number
    leadid: string
    log: string
    addby: string
    addon: string
}

export interface SheduleCallRequest {
    authDto: AuthDetails
    leadNo: string
    scheuledTime: Date
    description: string
    assignedStaff: number
    OriginalTime: string
    OriginalDate: Date
}

export interface MakeCallCenterAssign {
    email: string
    authDto: AuthDetails
    remark: string
    status: string
}


export interface NotinterestedDto
{
    // remark: string
    events : string
    email : string
    authDto : AuthDetails

}