import { Component, Inject, OnInit } from "@angular/core";
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Challenge } from '../../services/challenges/dto/challenge';

@Component({
  selector: 'challenge-dialog',
  templateUrl: './challenge-dialog.component.html'
  //styleUrls: ['./challenge-dialog.component.css']
})
export class ChallengeDialogComponent implements OnInit {
  form: FormGroup = new FormGroup({ title: new FormControl(''), description: new FormControl('') });

  title: string;
  description: string;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ChallengeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {

    this.title = data.title;
    this.description = data.description;
  }

  ngOnInit() {
    this.form = this.fb.group({
      title: [this.title, [Validators.required, Validators.maxLength(250)]],
      description: [this.description, [Validators.required, Validators.maxLength(1000)]],
    });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    this.dialogRef.close(this.form.value as Challenge);
  }

  close() {
    this.dialogRef.close(null);
  }
}
