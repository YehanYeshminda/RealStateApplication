import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { BehaviorSubject, Observable } from 'rxjs';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { PropertyDevView, propdev } from '../propdev';
@Injectable({
	providedIn: 'root'
})
export class PropertydevelopmentService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) { }

	private usernameImage: BehaviorSubject<string> = new BehaviorSubject<string>(
		''
	);

	getUserImage() {
		return this.usernameImage.asObservable();
	}

	setUserImage(image: string) {
		this.usernameImage.next(image);
	}

	getpropdev(): Observable<Root<PropertyDevView[]>> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<PropertyDevView[]>>(this.baseUrl + "PropertyDevelopment/getpropdev", auth);
	}

	getStaffNoAndIdComboData(): Observable<ComboInfo[]> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<ComboInfo[]>(this.baseUrl + "PropertyDevelopment/GetStaffNameIdAll", auth)
	}

	addpropdev(propdev: propdev) {
		return this.http.post<Root<propdev[]>>(
			this.baseUrl + 'PropertyDevelopment/insertpropdev',
			propdev
		);
	}

	updatepropdev(propdev: propdev) {
		return this.http.post<Root<propdev[]>>(
			this.baseUrl + 'PropertyDevelopment/updatepropdev',
			propdev
		);
	}

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'PropertyDevelopment/report');
	}

	cellreport(id: string): Observable<any> {
		return this.http.get(this.baseUrl + 'PropertyDevelopment/cellreport/' + id);
	}
}
