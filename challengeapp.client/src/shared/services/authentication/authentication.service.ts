import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { UserRegistrationDto } from "./dto/user-registration-dto";
import { UserLoginDto } from "./dto/user-login-dto";
import { UserLoginResult } from "./dto/user-login-result";
import { UserProfile } from "./dto/user-profile";
import { Router } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  constructor(private http: HttpClient) {
  }

  public registerUser = (body: UserRegistrationDto) => {
    return this.http.post<string>(`api/users/register`, body);
  }

  public loginUser = (body: UserLoginDto) => {
    return this.http.post<UserLoginResult>(`/api/users/login`, body);
  }

  public account = () => {
    return this.http.get<UserProfile>(`/api/users/manage/info`);
  }
}
