import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddNewLeadConversion, Bulkassign, CallInsightRequest, CallListViewData } from '../models/callinsignt';
import { Root } from 'src/app/shared/models/base';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CallCenterInfo, CallCenterRequest, LeadLogResponseByLeadNo, MakeCallCenterAssign, NotinterestedDto, SheduleCallRequest } from '../../shared/models/models';
import { LeadLogResponse } from '../../lead-forward/models/leadforward';
import { CallListPaginationAll, MakeCallResponse, MakeCenterPaginationRequest } from '../make-calls/models/ makecall';

@Injectable({
  providedIn: 'root'
})
export class CallCenterHttpService {
  baseurl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getAllCallInsignts(page: number = 1, pageSize: number = 5): Observable<Root<CallListViewData>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<CallListViewData>>(this.baseurl + "CallCenter/ContactListAllNew", auth, {
      params: {
        page: page,
        pageSize: pageSize
      }
    })
  }

  convertToLead(data: AddNewLeadConversion): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseurl + "CallCenter/AddNewConvertionLead", data)
  }

  assignCallInsights(data: CallInsightRequest): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseurl + "CallCenter/AssignCallInsignts", data)
  }

  assignbulk(data: Bulkassign): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseurl + "CallCenter/AssignTopCallInsights", data)
  }

  getAllCallInsigntsForUser(data: MakeCenterPaginationRequest): Observable<Root<CallListPaginationAll>> {
    return this.http.post<Root<CallListPaginationAll>>(this.baseurl + "CallCenter/CallListInsignts", data)
  }

  updateCallIsightStartTime(email: string): Observable<Root<string>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<string>>(this.baseurl + "CallCenter/UpdateCallTime", {
      authDto: auth,
      Email: email
    })
  }

  updateCallIsightEndTime(email: string): Observable<Root<string>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<string>>(this.baseurl + "CallCenter/UpdateCallEndTime", {
      authDto: auth,
      Email: email
    })
  }

  getLeadCallCenterInfo(leadNo: string): Observable<Root<CallCenterInfo>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<CallCenterInfo>>(this.baseurl + "auth/GetLeadInfoForDialog?leadNo=" + leadNo, auth)
  }

  updateLeadStatus(data: CallCenterRequest): Observable<Root<LeadLogResponse>> {
    return this.http.post<Root<LeadLogResponse>>(this.baseurl + "callcenter/UpdateLeadLog", data);
  }

  getLeadLogHistory(leadNo: string): Observable<Root<LeadLogResponseByLeadNo[]>> {
    return this.http.get<Root<LeadLogResponseByLeadNo[]>>(this.baseurl + "CallCenter/GetLeadLogByLeadNo?leadNo=" + leadNo)
  }

  updateScheduleForLead(data: SheduleCallRequest): Observable<Root<LeadLogResponseByLeadNo>> {
    return this.http.post<Root<LeadLogResponseByLeadNo>>(this.baseurl + "CallCenter/OnReScheduleCall", data)
  }

  addNewConvertionToCallInsignt(data: MakeCallCenterAssign): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseurl + "CallCenter/AddNewConvertionLead", data)
  }

  newDND(data: NotinterestedDto): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseurl + "CallCenter/Notinterested", data)
  }

  loadExistingLeadInformation(phone: string): Observable<Root<MakeCallResponse>> {
    return this.http.post<Root<MakeCallResponse>>(this.baseurl + "CallCenter/GetLeadInsightInformation", {
      PhoneNumber:phone
    })
  }
}
