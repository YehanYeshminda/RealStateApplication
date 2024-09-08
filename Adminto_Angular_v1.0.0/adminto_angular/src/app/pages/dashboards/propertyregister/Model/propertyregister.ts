export interface propertyregister {
    id: string;
    propertname: string;
    type: number;
    category: number;
    subcategory: number;
    city: number;
    nationality: number;
    address: string;
    geolocation: string;
    vender: number;
    costanually: number;
    othercost: number;
    rulesregulations: string;
    status: number;
    sellingprice: number;
    minsellingprice: number;
    anualcostforbuyer: number;
    deposit: number;
    contacttype: number;
    socialmedia: number;
    mainimg: string;
    otherimg: string;
    dateofpurchorrent: Date;
    renewdate: Date;
    venderpaymentdate: Date;
    paymentscheduleno: string;
}

export interface PropertyRegisterView {
    Id: string
    PropertyName: string
    PropertyType: string
    PropertyCatergory: string
    PropertySubCatergory: string
    TypeName: string
    Nationality: string
    Address: string
    geolocation: string
    SupplierName: string
    CostAnually: number
    Othercost: number
    Rulesregulations: string
    Status: number
    Sellingprice: number
    Minsellingprice: number
    Anualcostforbuyer: number
    Deposit: number
    Contacttype: number
    Socialmedia: number
    Mainimg: string
    Otherimg: string
    Dateofpurchorrent: string
    Renewdate: string
    Venderpaymentdate: string
    Addon: string
    Paymentscheduleno: string
    AddBy: string
}

export interface VRegListAll {
    data: PropertyRegisterView[]
    totalCountPages: number
    totalData: number
  }