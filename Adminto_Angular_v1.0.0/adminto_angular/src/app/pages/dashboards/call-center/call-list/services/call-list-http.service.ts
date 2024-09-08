import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, tap } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { environment } from 'src/environments/environment';
import { CallListPaginationData, calllist } from '../models/calllist';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';

@Injectable({
  providedIn: 'root'
})
export class CallListHttpService {
  baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) {}
	

	  
	getcalllist(page: number = 1, pageSize: number = 5): Observable<Root<CallListPaginationData>> {
		const auth: AuthDetails = GetAuthDetails();
		return this.http.post<Root<CallListPaginationData>>(this.baseUrl + "CallCenter/CallListInsigntsAllNew", auth, {
			params: {
				page: page,
				pageSize: pageSize
			  }
		});
	}

	addcalllist(calllist: calllist) {
		return this.http.post<Root<CallListPaginationData>>(
			this.baseUrl + 'CampainH/insertCampainH',
			calllist
		);
	}

	uploadExcelFile(file: File, name: string, email: string, phone: string): Observable<any> {
		const formData = new FormData();
		formData.append('file', file);
		formData.append('phoneNoCol', phone);
		formData.append('emailCol', email);
		formData.append('customerName', name);

		return this.http.post<any>(`${this.baseUrl}upload/excel`, formData);
	}

	loadAllCallListDataTitles(file: File): Observable<Root<string[]>> {
		const formData = new FormData();
		formData.append('file', file);
		return this.http.post<Root<string[]>>(this.baseUrl + "upload/excel/columns", formData);
	}

	loadAllCallListData(file: File): Observable<Root<string[]>> {
		const formData = new FormData();
		formData.append('file', file);
		return this.http.post<Root<string[]>>(this.baseUrl + "upload/excel/columndata", formData);
	}

	getcolumns() {
		return this.http.get<Root<string[]>>(this.baseUrl + 'upload/crmColumns')
	  }
	  
	  
}
