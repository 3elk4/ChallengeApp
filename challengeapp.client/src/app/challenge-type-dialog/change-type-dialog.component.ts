import { Component, Inject, OnInit } from "@angular/core";
import { ChallengeType } from "../../services/challenges/dto/challenge-type";
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";

@Component({
  selector: 'challenge-type-dialog',
  templateUrl: './change-type-dialog.component.html'
  //styleUrls: ['./challenge-dialog.component.css']
})
export class ChangeTypeDialogComponent implements OnInit {
  form: FormGroup = new FormGroup({ type: new FormControl(0) });

  type: ChallengeType;

  StringIsNumber = (value: string) => isNaN(Number(value)) === false;

  challengeTypes = Object.keys(ChallengeType).filter(this.StringIsNumber).map(
    (type) => ({ key: type, value: (ChallengeType as any)[type] })
  );

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ChangeTypeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {

    this.type = data.type;
  }

  ngOnInit() {
    this.form = this.fb.group({
      type: [this.type, [Validators.required]]
    });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    this.f['type'].setValue(parseInt(this.f['type'].value));

    this.dialogRef.close(this.form.value as ChallengeType);
  }

  close() {
    this.dialogRef.close(null);
  }
}
