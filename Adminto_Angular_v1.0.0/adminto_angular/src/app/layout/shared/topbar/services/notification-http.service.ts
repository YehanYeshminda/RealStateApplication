import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { environment } from 'src/environments/environment';
import { AllNotifications } from '../models/notificationlist';

@Injectable({
  providedIn: 'root'
})
export class NotificationHttpService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllNotificationsForUser(): Observable<Root<AllNotifications[]>> {
    const auth: any = GetAuthDetails();
    return this.http.post<Root<AllNotifications[]>>(this.baseUrl + "Notification/AllNoti", auth);
  }
}
