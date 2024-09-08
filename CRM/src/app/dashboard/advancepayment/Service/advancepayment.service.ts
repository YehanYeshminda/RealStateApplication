import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { BehaviorSubject, Observable } from 'rxjs';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { VadvPayment, advPayment } from '../advpayment';
@Injectable({
	providedIn: 'root'
})
export class AdvancepaymentService {
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

	getadvpayment(): Observable<Root<VadvPayment[]>> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<VadvPayment[]>>(this.baseUrl + "AdvPayment/getadvpayment", auth);
	}

	getStaffNoAndIdComboData(): Observable<ComboInfo[]> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<ComboInfo[]>(this.baseUrl + "AdvPayment/GetStaffNameIdAll", auth)
	}

	addadvpayment(advpayment: advPayment) {
		return this.http.post<Root<advPayment[]>>(
			this.baseUrl + 'AdvPayment/insertadvpayment',
			advpayment
		);
	}

	updateadvpayment(advpayment: advPayment) {
		return this.http.post<Root<advPayment[]>>(
			this.baseUrl + 'AdvPayment/updateadvpayment',
			advpayment
		);
	}

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'AdvPayment/report');
	}

	cellreport(id: number): Observable<any> {
		return this.http.get(this.baseUrl + 'AdvPayment/cellreport/' + id);
	}
}
