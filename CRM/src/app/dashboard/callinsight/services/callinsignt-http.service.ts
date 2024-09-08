import { Injectable } from '@angular/core';
import { Root } from 'src/app/shared/models/baseResponse';
import { Bulkassign, CallInsight, CallInsightRequest } from '../models/callinsight';
import { environment } from 'src/environments/environment.development';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CallinsigntHttpService {
  baseurl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllCallInsignts(): Observable<Root<CallInsight[]>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<CallInsight[]>>(this.baseurl + "CallCenter/ContactListAll", auth)
  }

  assignCallInsights(data: CallInsightRequest): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseurl + "CallCenter/AssignCallInsignts", data)
  }

  assignbulk(data: Bulkassign): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseurl + "CallCenter/AssignTopCallInsights", data)
  }

  getAllCallInsigntsForUser(): Observable<Root<CallInsight[]>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<CallInsight[]>>(this.baseurl + "CallCenter/CallListInsignts", auth)
  }

  updateCallIsightStartTime(email: string): Observable<Root<string>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<string>>(this.baseurl + "CallCenter/UpdateCallTime", auth, {
      params: {
        email: email
      }
    })
  }

  updateCallIsightEndTime(email: string): Observable<Root<string>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<string>>(this.baseurl + "CallCenter/UpdateCallEndTime", auth, {
      params: {
        email: email
      }
    })
  }
}
