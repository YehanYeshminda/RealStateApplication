import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { environment } from 'src/environments/environment';
import { PaginationResult, StaffPerformancePaginationResult, StaffPerformanceRequest } from '../Models/staffperfo';
import { HtmlResponse } from '../../staff/models/staff';

@Injectable({
  providedIn: 'root'
})
export class StaffperformanceService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllstaffperformances(data: StaffPerformanceRequest, startDate: Date, endDate: Date, staffId: number): Observable<Root<PaginationResult[]>> {
    const url = `${this.baseUrl}CallCenter/GettingCallandleadscount?startDate=${startDate}&endDate=${endDate}&staffId=${staffId}`;
    return this.http.post<Root<PaginationResult[]>>(url, data);
  }

}
