import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { HtmlResponse, StaffNew, VStaffListAll } from '../models/staff';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AssignedCallsPersonInsignt, AssignedCallsPersonInsigntResultAll } from './assignedcallsperson';



@Injectable({
  providedIn: 'root'
})
export class StaffHttpService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  	getstaff(page: number = 1, pageSize: number = 5): Observable<Root<VStaffListAll>> {
		const auth: AuthDetails = GetAuthDetails();
		return this.http.post<Root<VStaffListAll>>(this.baseUrl + "staff/getstaffNew", auth, {
		params: {
			page: page,
			pageSize: pageSize
		}
		});
	}

	addstaff(staff: FormData) {
		return this.http.post<Root<StaffNew[]>>(
			this.baseUrl + 'staff/insertstaff',
			staff
		);
	}

	updatestaff(staff: FormData) {
		return this.http.post<Root<StaffNew[]>>(
			this.baseUrl + 'staff/updatestaff',
			staff
		);
	}

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'staff/report');
	}
	  
	cellreport(id: number): Observable<any> {
		return this.http.get(this.baseUrl + 'staff/cellreport/' + id);
	}

	deleteStaff(id: number): Observable<Root<String>> {
		const auth: AuthDetails = GetAuthDetails();
		return this.http.post<Root<string>>(this.baseUrl + "staff/DeleteStaff?id=" + id, auth);
	}

	assignedCallsForStaffToday(id: number, page: number = 1, pageSize: number = 10): Observable<Root<AssignedCallsPersonInsigntResultAll>> {
		return this.http.get<Root<AssignedCallsPersonInsigntResultAll>>(this.baseUrl + "staff/GetStaffCalls", {
			params: {
				staffId: id,
				page: page,
				pageSize: pageSize
			}
		});
	}
	
	removeAssignedCallsFromUser(staffId: string, count: number) {
		return this.http.post<Root<number>>(this.baseUrl + "staff/UpdateStaffCallsStatus", {}, {
			params: {
				staffId: staffId,
				count: count
			}
		});
	}
}
