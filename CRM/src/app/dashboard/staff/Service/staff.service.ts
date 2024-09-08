import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { VStaffList, staff } from '../staff';
import { getAuthDetails } from 'src/app/shared/methods/method';

@Injectable({
  providedIn: 'root'
})
export class StaffService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) {}

	getstaff(): Observable<Root<VStaffList[]>> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<VStaffList[]>>(this.baseUrl + "staff/getstaff", auth);
	}

	// addstaff(staff: staff) {
	// 	return this.http.post<Root<staff[]>>(
	// 		this.baseUrl + 'staff/insertstaff',
	// 		staff
	// 	);
	// }
	addstaff(staff: FormData) {
		return this.http.post<Root<staff[]>>(
			this.baseUrl + 'staff/insertstaff',
			staff
		);
	}


	// updatestaff(staff: staff) {
	// 	return this.http.post<Root<staff[]>>(
	// 		this.baseUrl + 'staff/updatestaff',
	// 		staff
	// 	);
	// }
	updatestaff(staff: FormData) {
		return this.http.post<Root<staff[]>>(
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
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<string>>(this.baseUrl + "staff/DeleteStaff?id=" + id, auth);
	}
}
