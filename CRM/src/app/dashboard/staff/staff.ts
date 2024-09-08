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


export interface stafflist{
    id :number;
    name :string;
    designation :string;
    mobileno :string;
    parentid :number;
    addby :string;
    addon :Date;
    status :number;
    userid :number;
}

export interface VStaffList {
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
    VisaIssuedate : Date 
    Passport : string
    Userimage : string
  }


  

