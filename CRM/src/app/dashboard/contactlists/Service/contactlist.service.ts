import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { calllist } from '../contactlist';
@Injectable({
  providedIn: 'root'
})
export class ContactlistService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) {}

	getcalllist(): Observable<Root<calllist[]>> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<calllist[]>>(this.baseUrl + "CallCenter/CallListInsigntsAll", auth);
	}

	addcalllist(calllist: calllist) {
		return this.http.post<Root<calllist[]>>(
			this.baseUrl + 'CampainH/insertCampainH',
			calllist
		);
	}

  uploadExcelFile(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<any>(`${this.baseUrl}upload/excel`, formData);
  }

}
