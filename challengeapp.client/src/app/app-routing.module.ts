import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { RegisterUserComponent } from './authentication/register/register.component';
import { LoginUserComponent } from './authentication/login/login.component';
import { ChallengesComponent } from './challenges/challenges.component';
import { PageNotFoundComponent } from './pagenotfound/pagenotfound.component';
import { UserProfileComponent } from './authentication/user/user-profile.component';
import { LogoutUserComponent } from './authentication/logout/logout.component';
import { ChallengeDetailsComponent } from './challengedetails/challenge-details.component';

const routes: Routes = [
  //{ path: 'error/:status', component: ErrorStatusComponent },
  { path: '', component: HomeComponent },
  { path: 'fetch-data', component: FetchDataComponent },
  { path: 'challenges', component: ChallengesComponent },
  { path: 'challenges/:id', component: ChallengeDetailsComponent },
  { path: 'profile', component: UserProfileComponent },
  {
    path: 'authentication', children: [
      { path: 'signup', component: RegisterUserComponent },
      { path: 'login', component: LoginUserComponent },
      { path: 'logout', component: LogoutUserComponent }
    ]
  },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
