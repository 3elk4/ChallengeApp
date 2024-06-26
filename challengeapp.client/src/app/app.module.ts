import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { RegisterUserComponent } from './authentication/register/register.component';
import { LoginUserComponent } from './authentication/login/login.component';
import { ChallengesComponent } from './challenges/challenges.component';
import { AuthInterceptor } from '../shared/services/authentication/auth.intercepor';
import { PageNotFoundComponent } from './pagenotfound/pagenotfound.component';
import { UserProfileComponent } from './authentication/user/user-profile.component';
import { LogoutUserComponent } from './authentication/logout/logout.component';
import { ErrorInterceptor } from '../shared/services/errorstatus/error.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { ChallengeDialogComponent } from './challenge-dialog/challenge-dialog.component';
import { MAT_DIALOG_DEFAULT_OPTIONS, MatDialogModule } from '@angular/material/dialog'; 
import { ChoreDialogComponent } from './chore-dialog/chore-dialog.component';
import { ChallengeDetailsComponent } from './challengedetails/challenge-details.component';
import { ChangeTypeDialogComponent } from './challenge-type-dialog/change-type-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    PageNotFoundComponent,
    HomeComponent,
    NavMenuComponent,
    FetchDataComponent,
    ChallengesComponent,
    ChallengeDetailsComponent,
    RegisterUserComponent,
    LoginUserComponent,
    UserProfileComponent,
    LogoutUserComponent,
    ChallengeDialogComponent,
    ChoreDialogComponent,
    ChangeTypeDialogComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    MatDialogModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: { width: '300px', position: { 'top': '0', 'left': '10' } } }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
