import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UpdateCountHttpService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  updateCount(count: number, type: string, staffId: number): Observable<Root<string>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.put<Root<string>>(this.baseUrl + "Chart/UpdateCountDaily", auth, {
      params: {
        count: count,
        type: type,
        staffId: staffId
      }
    })
  }
}
