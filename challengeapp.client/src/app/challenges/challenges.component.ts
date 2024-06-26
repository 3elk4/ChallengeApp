import { Component, Inject, OnInit } from '@angular/core';
import { ChallengeService } from '../../services/challenges/challenge.service';
import { Challenge } from '../../services/challenges/dto/challenge';
import { Router } from '@angular/router';
import 'ngx-toastr/toastr';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog'; 
import { ChallengeDialogComponent } from '../challenge-dialog/challenge-dialog.component';
import { HttpErrorResponse } from '@angular/common/http';
import { ChallengeType } from '../../services/challenges/dto/challenge-type';
import { ChangeTypeDialogComponent } from '../challenge-type-dialog/change-type-dialog.component';

@Component({
  selector: 'app-challenges',
  templateUrl: './challenges.component.html'
})

export class ChallengesComponent implements OnInit {
  public challenges: Challenge[] = [];
  public pageNumber: number = 0;
  public totalPages: number = 0;
  public totalCount: number = 0;

  constructor(private challengeService: ChallengeService,
              private router: Router,
              private toastr: ToastrService,
              public dialog: MatDialog) { }

  ngOnInit(): void {
    this.challengeService.challenges().subscribe(
      (result) => {
        this.challenges = result.items;
        this.pageNumber = result.pageNumber;
        this.totalPages = result.totalPages;
        this.totalCount = result.totalCount;
      }
    );
  }

  showDetails(id: string) {
    this.router.navigateByUrl(`/challenges/${id}`);
  }

  createChallenge() {
    let dialogRef = this.dialog.open(ChallengeDialogComponent, {
      data: { title: '', description: '' }
    });

    dialogRef.afterClosed().subscribe((data: Challenge) => {
      if (data === null) return;

      this.challengeService.addNew(data).subscribe(
        res => {
          this.toastr.success('Challenge created successfully');
          this.router.navigateByUrl(`/challenges/${res}`);
        }
      )
    });
  }

  editChallenge(challenge : Challenge) {
    let dialogRef = this.dialog.open(ChallengeDialogComponent, {
      data: { title: challenge.title, description: challenge.description }
    });

    dialogRef.afterClosed().subscribe((data: Challenge) => {
      if (data === null) return;

      data.id = challenge.id;

      this.challengeService.edit(challenge.id, data).subscribe(
        _ => {
          this.toastr.success('Challenge updated successfully');
          this.router.navigateByUrl(`/challenges/${challenge.id}`);
        },
        (error: HttpErrorResponse) => {
          if (error.status === 403) {
            this.toastr.error(error.headers.get('x-forbidden-reason') || "Server error");
          }
        }
      )
    });
  }

  deleteChallenge(id: string) {
    if (confirm("Are you sure to delete challenge with id " + id)) {
      this.challengeService.delete(id).subscribe(
        _ => window.location.reload(),
        (error: HttpErrorResponse) => {
          if (error.status === 403) {
            this.toastr.error(error.headers.get('x-forbidden-reason') || "Server error");
          }
        }
      );
    }
  }

  copyChallenge(id: string) {
    this.challengeService.copy(id).subscribe(
      _ => window.location.reload(),
      (error: HttpErrorResponse) => {
        if (error.status === 403) {
          this.toastr.error(error.headers.get('x-forbidden-reason') || "Server error");
        }
      }
    );
  }

  archiveChallenge(id: string) {
    this.challengeService.archive(id).subscribe(
      _ => window.location.reload(),
      (error: HttpErrorResponse) => {
        if (error.status === 403) {
          this.toastr.error(error.headers.get('x-forbidden-reason') || "Server error");
        }
      }
    );
  }

  unarchiveChallenge(id: string) {
    this.challengeService.unarchive(id).subscribe(
      _ => window.location.reload(),
      (error: HttpErrorResponse) => {
        if (error.status === 403) {
          this.toastr.error(error.headers.get('x-forbidden-reason') || "Server error");
        }
      }
    );
  }

  changeTypeChallenge(id: string, challenge: Challenge) {

    let dialogRef = this.dialog.open(ChangeTypeDialogComponent, {
      data: { type: challenge.type.toString() }
    });

    dialogRef.afterClosed().subscribe((data: ChallengeType) => {
      if (data === null) return;

      this.challengeService.changeType(id, data).subscribe(
        _ => {
          this.toastr.success('Challenge type changed successfully');
          window.location.reload()
        },
        (error: HttpErrorResponse) => {
          if (error.status === 403) {
            this.toastr.error(error.headers.get('x-forbidden-reason') || "Server error");
          }
        }
      );
    });
  }
}

