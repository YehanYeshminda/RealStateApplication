import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { CompanyInfo } from './company';

@Injectable({
	providedIn: 'root',
})
export class UserService {
	baseUrl = environment.apiUrl;

	constructor(private http: HttpClient) {}

	private usernameSubject: BehaviorSubject<string> =
		new BehaviorSubject<string>('');

	private usernameImage: BehaviorSubject<string> = new BehaviorSubject<string>(
		''
	);

	private companyName: BehaviorSubject<string> = new BehaviorSubject<string>(
		''
	);

	getUsernameObservable() {
		return this.usernameSubject.asObservable();
	}

	getUserImage() {
		return this.usernameImage.asObservable();
	}

	getCompanyName() {
		return this.companyName.asObservable();
	}

	setUsername(username: string) {
		this.usernameSubject.next(username);
	}

	setUserImage(image: string) {
		this.usernameImage.next(image);
	}

	setCompanyName(companyName: string) {
		this.companyName.next(companyName);
	}

	getCompanyNameValue(companyId: number): Observable<CompanyInfo> {
		return this.http.get<CompanyInfo>(
			this.baseUrl + 'company/GetSingle?cId=' + companyId
		);
	}
}
