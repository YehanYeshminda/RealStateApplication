import { AuthDetails } from "src/app/shared/models/methods"

export interface VStaffListAll {
    data: Staff[]
    totalCountPages: number
    totalData: number
  }
  
  export interface Staff {
    Id: number
    Name: string
    Designation: string
    Mobileno: string
    Addon: string
    Status: number
    Parentid: string
    FirstName: string
    LastName: string
    Email: string
    Password: string
    VisaIssuedate: string
    Passport: string
    Userimage: string
  }

  export interface StaffNew{
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

export interface HtmlResponse {
	content: string;
}

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