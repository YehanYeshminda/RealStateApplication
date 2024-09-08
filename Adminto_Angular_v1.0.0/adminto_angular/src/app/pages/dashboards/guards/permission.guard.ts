import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { UserPermissionHttpService } from '../user-permissions/services/user-permission-http.service';
import { Observable, catchError, map, of, tap } from 'rxjs';
import { errorNotification } from '../shared/notifications/notification';

@Injectable({
  providedIn: 'root'
})
export class PermissionGuard implements CanActivate {

  constructor(private userPermissionHtppService: UserPermissionHttpService, private router: Router) {
    window.addEventListener('beforeunload', () => {
      sessionStorage.removeItem('userPermissions');
    });
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    const urlKey = route.data['key'];
    
    const cachedPermissions = sessionStorage.getItem('userPermissions');
    if (cachedPermissions) {
      const permissions = JSON.parse(cachedPermissions);
      return this.checkPermissions(permissions, urlKey);
    } else {
      return this.userPermissionHtppService.getAllUserPermissionsForHome().pipe(
        tap(response => {
          if (response.isSuccess) {
            const permissions = response.result;
            sessionStorage.setItem('userPermissions', JSON.stringify(permissions));
          } else {
            errorNotification("Failed to fetch user permissions");
          }
        }),
        map(response => response.isSuccess && response.result.includes(urlKey)),
        catchError(() => of(false)),
        tap(hasPermission => {
          if (!hasPermission) {
            errorNotification("Insufficient Permission for routing into this URL");
            this.router.navigate(['/dashboard']);
          }
        })
      );
    }
  }

  private checkPermissions(permissions: string[], urlKey: string): Observable<boolean> {
    return of(permissions.includes(urlKey));
  }
}
