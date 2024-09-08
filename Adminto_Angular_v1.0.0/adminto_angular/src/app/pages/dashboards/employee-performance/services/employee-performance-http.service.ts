import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Root } from 'src/app/shared/models/base';
import { environment } from 'src/environments/environment';
import { EmployeePerformance } from '../models/performance';
import { Observable } from 'rxjs';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';

@Injectable({
  providedIn: 'root'
})
export class EmployeePerformanceHttpService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getAllEmployeePerformance(): Observable<Root<EmployeePerformance[]>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<EmployeePerformance[]>>(this.baseUrl + "performance/all", auth)
  }

  getAllEmployeePerformanceWeekly(): Observable<Root<EmployeePerformance[]>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<EmployeePerformance[]>>(this.baseUrl + "performance/allWeekly", auth)
  }

  getAllEmployeePerformanceMonthly(): Observable<Root<EmployeePerformance[]>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<EmployeePerformance[]>>(this.baseUrl + "performance/allMonthly", auth)
  }

  generateEmployeePerformanceReportToday(): Observable<Root<string>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<string>>(this.baseUrl + "performance/reportToday", auth)
  }

  generateEmployeePerformanceReportWeekly(): Observable<Root<string>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<string>>(this.baseUrl + "performance/reportWeekly", auth)
  }
}
