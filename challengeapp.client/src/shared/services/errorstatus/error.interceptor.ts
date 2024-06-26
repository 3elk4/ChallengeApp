import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { Observable, catchError, of, throwError } from "rxjs";

@Injectable({ providedIn: 'root' })
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private toastr: ToastrService) { }

  private handleAuthError(err: HttpErrorResponse): Observable<any> {
    if (err.status === 401) {
      this.router.navigateByUrl(`/authentication/login`);
    }
    else if (err.status === 403) {
      if (!err.headers.has('x-forbidden-reason')) {
        this.router.navigateByUrl(`/authentication/login`);
        return of('Forbidden');
      }
    }
    else if (err.status === 404) {
      this.router.navigateByUrl(`/**`);
      return of('Not found');
    }
    else if (err.status === 500) {
      this.toastr.error("Server error");
      return of('Server Error');
    }

    return throwError(err);
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(catchError(x => this.handleAuthError(x)));
  }
}
