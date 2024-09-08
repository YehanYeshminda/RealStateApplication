import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { environment } from 'src/environments/environment.development';
import { CalenderData, ChartData, DashboaordLeadCount } from '../models/chart'

@Injectable({
  providedIn: 'root'
})
export class ChartHttpService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getLeadLineChartData(): Observable<Root<ChartData>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<ChartData>>(this.baseUrl + "auth/GetLeadsChartData", auth)
  }

  getCalenderData(): Observable<Root<CalenderData[]>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<CalenderData[]>>(this.baseUrl + "meetingscheduler/GetMeetingInfoForCalender", auth)
  }

  getCountForDashboard(): Observable<Root<DashboaordLeadCount>> {
    return this.http.get<Root<DashboaordLeadCount>>(this.baseUrl + "auth/GetLeadStatusResults")
  }
}
