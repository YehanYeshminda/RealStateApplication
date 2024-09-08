import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Root } from 'src/app/shared/models/base';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';
import { environment } from 'src/environments/environment';
import { ChartStatisticData, UpdateOldPassword } from '../models/chartdata';
import { Observable } from 'rxjs';
import { BarChartData, ChartPieData } from '../dashboard.model';
import { BarChartDataMulti } from '../data';
import { TodayYesterdayPercentage } from './models/chart';

@Injectable({
  providedIn: 'root'
})
export class ChartDataHttpService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getBarChartStatisticsData(): Observable<Root<BarChartData>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<BarChartData>>(this.baseUrl +"Chart/GetConversionsForDates" , auth);
  }

  getBarCallsAssignedChartStatisticsData(): Observable<Root<BarChartData>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<BarChartData>>(this.baseUrl +"Chart/GetAssignedConversionsForDates" , auth);
  }

  getLeadChartStatisticsData(): Observable<Root<ChartStatisticData>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<ChartStatisticData>>(this.baseUrl +"Chart/GetLeadStatisticData" , auth);
  }

  getLostLeadChartStatisticsData(): Observable<Root<ChartStatisticData>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<ChartStatisticData>>(this.baseUrl +"Chart/GetLeadLostData" , auth);
  }

  getCallInsigntStatisticsData(): Observable<Root<ChartStatisticData>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<ChartStatisticData>>(this.baseUrl + "Chart/GetCallsStatisticData", auth);
  }

  getPieChartStatisticsData(): Observable<Root<ChartPieData>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<ChartPieData>>(this.baseUrl + "Chart/GetPieChartData", auth);
  }


  getCallsLeftStatisticsData(): Observable<Root<ChartStatisticData>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<ChartStatisticData>>(this.baseUrl + "Chart/GetCallAvailableStatisticData", auth);
  }

  checkIfFirstLogin(): Observable<Root<string>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<string>>(this.baseUrl + "auth/LoginFirst", auth)
  }

  passwordReset(data: UpdateOldPassword): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseUrl + "auth/PasswordReset", data)
  }

  passwordResetCancel(): Observable<Root<string>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<string>>(this.baseUrl + "auth/UpdateCancel", auth)
  }

  getBarChartMultiData(): Observable<Root<BarChartDataMulti>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<BarChartDataMulti>>(this.baseUrl + "Chart/GetConversionsForTimeSlots", auth)
  }

  getTodayYesterdayPerformance(): Observable<Root<TodayYesterdayPercentage>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<TodayYesterdayPercentage>>(this.baseUrl + "Chart/CallsMadeAccordingToYesterday", auth)
  }
}
