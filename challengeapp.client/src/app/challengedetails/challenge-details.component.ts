import { Component, OnInit } from "@angular/core";
import { ChallengeService } from "../../services/challenges/challenge.service";
import { ChallengeDetails } from "../../services/challenges/dto/challenge-details";
import { ActivatedRoute } from "@angular/router";
import { HttpErrorResponse } from "@angular/common/http";
import { ToastrService } from "ngx-toastr";
import { Chore } from "../../services/chores/dto/chore";
import { ChoreService } from "../../services/chores/chore.service";
import { MatDialog } from "@angular/material/dialog";
import { ChoreDialogComponent } from "../chore-dialog/chore-dialog.component";

@Component({
  selector: 'app-challenge-details',
  templateUrl: './challenge-details.component.html'
})

export class ChallengeDetailsComponent implements OnInit {
  public challangeDetails: ChallengeDetails = { title: "", description: "", type: 0, chores: [] };

  private id : string | null = "";

  constructor(private route: ActivatedRoute,
    private challengeService: ChallengeService,
    private choreService: ChoreService,
    private toastr: ToastrService,
    public dialog: MatDialog) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');

    this.challengeService.challengeDetails(this.id).subscribe(
      (result) => {
        this.challangeDetails = result;
      }
    );
  }

  createChore() {
    let dialogRef = this.dialog.open(ChoreDialogComponent, {
      data: {
        title: '', description: '', points: 0, difficulty: '0'
      }
    });

    dialogRef.afterClosed().subscribe((data: Chore) => {
      if (data === null) return;

      data.challengeId = this.id || "";

      this.choreService.addNew(data).subscribe(
        res => {
          this.toastr.success('Chore created successfully');
          window.location.reload();
        }
      )
    });
  }

  editChore(chore: Chore) {
    let dialogRef = this.dialog.open(ChoreDialogComponent, {
      data: {
        title: chore.title, description: chore.description, points: chore.points, difficulty: chore.difficulty.toString()
      }
    });

    dialogRef.afterClosed().subscribe((data: Chore) => {
      if (data === null) return;

      data.id = chore.id;

      this.choreService.edit(chore.id, data).subscribe(
        _ => {
          this.toastr.success('Chore updated successfully');
          window.location.reload();
        },
        (error: HttpErrorResponse) => {
          if (error.status === 403) {
            this.toastr.error(error.headers.get('x-forbidden-reason') || "Server error");
          }
        }
      )
    });
  }

  deleteChore(id: string) {
    if (confirm("Are you sure to delete chore with id " + id)) {
      this.choreService.delete(id).subscribe(
        _ => window.location.reload(),
        (error: HttpErrorResponse) => {
          if (error.status === 403) {
            this.toastr.error(error.headers.get('x-forbidden-reason') || "Server error");
          }
        }
      );
    }
  }

  completeChore(id: string) {
    this.choreService.complete(id).subscribe(
      _ => window.location.reload(),
      (error: HttpErrorResponse) => {
        if (error.status === 403) {
          this.toastr.error(error.headers.get('x-forbidden-reason') || "Server error");
        }
      }
    );
  }
}
