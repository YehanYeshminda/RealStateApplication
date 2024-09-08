import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { environment } from 'src/environments/environment.development';
import { CallCenterInfo, CallCenterRequest, LeadLogResponseByLeadNo, MakeCallCenterAssign, NotinterestedDto, SheduleCallRequest } from '../models/callcenter';
import { Root } from 'src/app/shared/models/baseResponse';
import { LeadLogResponse } from '../../leadsforward/models/leadforward';

@Injectable({
  providedIn: 'root'
})
export class CallcenterService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getLeadCallCenterInfo(leadNo: string): Observable<Root<CallCenterInfo>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<CallCenterInfo>>(this.baseUrl + "auth/GetLeadInfoForDialog?leadNo=" + leadNo, auth)
  }

  updateLeadStatus(data: CallCenterRequest): Observable<Root<LeadLogResponse>> {
    return this.http.post<Root<LeadLogResponse>>(this.baseUrl + "callcenter/UpdateLeadLog", data);
  }

  getLeadLogHistory(leadNo: string): Observable<Root<LeadLogResponseByLeadNo[]>> {
    return this.http.get<Root<LeadLogResponseByLeadNo[]>>(this.baseUrl + "CallCenter/GetLeadLogByLeadNo?leadNo=" + leadNo)
  }

  updateScheduleForLead(data: SheduleCallRequest): Observable<Root<LeadLogResponseByLeadNo>> {
    return this.http.post<Root<LeadLogResponseByLeadNo>>(this.baseUrl + "CallCenter/OnReScheduleCall", data)
  }

  addNewConvertionToCallInsignt(data: MakeCallCenterAssign): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseUrl + "CallCenter/AddNewConvertionLead", data)
  }

  newDND(data: NotinterestedDto): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseUrl + "CallCenter/Notinterested", data)
  }
}
