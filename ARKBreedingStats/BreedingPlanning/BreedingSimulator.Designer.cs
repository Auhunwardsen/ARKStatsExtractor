namespace ARKBreedingStats.BreedingPlanning
{
    partial class BreedingSimulator
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            lbMotherLabel = new System.Windows.Forms.Label();
            cbMother = new System.Windows.Forms.ComboBox();
            lbFatherLabel = new System.Windows.Forms.Label();
            cbFather = new System.Windows.Forms.ComboBox();
            lbHint = new System.Windows.Forms.Label();
            lbCaptionMother = new System.Windows.Forms.Label();
            pedigreeCreatureMother = new ARKBreedingStats.Pedigree.PedigreeCreature();
            lbCaptionFather = new System.Windows.Forms.Label();
            pedigreeCreatureFather = new ARKBreedingStats.Pedigree.PedigreeCreature();
            lbCaptionBest = new System.Windows.Forms.Label();
            pedigreeCreatureBest = new ARKBreedingStats.Pedigree.PedigreeCreature();
            lbGraphCaption = new System.Windows.Forms.Label();
            offspringPossibilities1 = new ARKBreedingStats.OffspringPossibilities();
            lbMutationProbability = new System.Windows.Forms.Label();
            SuspendLayout();
            //
            // lbMotherLabel
            //
            lbMotherLabel.AutoSize = true;
            lbMotherLabel.Location = new System.Drawing.Point(10, 14);
            lbMotherLabel.Name = "lbMotherLabel";
            lbMotherLabel.Size = new System.Drawing.Size(50, 15);
            lbMotherLabel.TabIndex = 0;
            lbMotherLabel.Text = "Mother:";
            //
            // cbMother
            //
            cbMother.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbMother.FormattingEnabled = true;
            cbMother.Location = new System.Drawing.Point(75, 10);
            cbMother.Name = "cbMother";
            cbMother.Size = new System.Drawing.Size(300, 23);
            cbMother.TabIndex = 1;
            //
            // lbFatherLabel
            //
            lbFatherLabel.AutoSize = true;
            lbFatherLabel.Location = new System.Drawing.Point(400, 14);
            lbFatherLabel.Name = "lbFatherLabel";
            lbFatherLabel.Size = new System.Drawing.Size(46, 15);
            lbFatherLabel.TabIndex = 2;
            lbFatherLabel.Text = "Father:";
            //
            // cbFather
            //
            cbFather.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbFather.FormattingEnabled = true;
            cbFather.Location = new System.Drawing.Point(460, 10);
            cbFather.Name = "cbFather";
            cbFather.Size = new System.Drawing.Size(300, 23);
            cbFather.TabIndex = 3;
            //
            // lbHint
            //
            lbHint.AutoSize = true;
            lbHint.Location = new System.Drawing.Point(10, 45);
            lbHint.Name = "lbHint";
            lbHint.Size = new System.Drawing.Size(420, 15);
            lbHint.TabIndex = 4;
            lbHint.Text = "Select a Mother and a Father of the same species to see the possible outcomes.";
            //
            // lbCaptionMother
            //
            lbCaptionMother.AutoSize = true;
            lbCaptionMother.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lbCaptionMother.Location = new System.Drawing.Point(10, 82);
            lbCaptionMother.Name = "lbCaptionMother";
            lbCaptionMother.Size = new System.Drawing.Size(53, 15);
            lbCaptionMother.TabIndex = 5;
            lbCaptionMother.Text = "Mother";
            //
            // pedigreeCreatureMother
            //
            pedigreeCreatureMother.Location = new System.Drawing.Point(10, 102);
            pedigreeCreatureMother.Name = "pedigreeCreatureMother";
            pedigreeCreatureMother.Size = new System.Drawing.Size(379, 55);
            pedigreeCreatureMother.TabIndex = 6;
            //
            // lbCaptionFather
            //
            lbCaptionFather.AutoSize = true;
            lbCaptionFather.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lbCaptionFather.Location = new System.Drawing.Point(10, 167);
            lbCaptionFather.Name = "lbCaptionFather";
            lbCaptionFather.Size = new System.Drawing.Size(47, 15);
            lbCaptionFather.TabIndex = 7;
            lbCaptionFather.Text = "Father";
            //
            // pedigreeCreatureFather
            //
            pedigreeCreatureFather.Location = new System.Drawing.Point(10, 187);
            pedigreeCreatureFather.Name = "pedigreeCreatureFather";
            pedigreeCreatureFather.Size = new System.Drawing.Size(379, 55);
            pedigreeCreatureFather.TabIndex = 8;
            //
            // lbCaptionBest
            //
            lbCaptionBest.AutoSize = true;
            lbCaptionBest.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lbCaptionBest.Location = new System.Drawing.Point(10, 252);
            lbCaptionBest.Name = "lbCaptionBest";
            lbCaptionBest.Size = new System.Drawing.Size(146, 15);
            lbCaptionBest.TabIndex = 9;
            lbCaptionBest.Text = "Best possible offspring";
            //
            // pedigreeCreatureBest
            //
            pedigreeCreatureBest.Location = new System.Drawing.Point(10, 272);
            pedigreeCreatureBest.Name = "pedigreeCreatureBest";
            pedigreeCreatureBest.Size = new System.Drawing.Size(379, 55);
            pedigreeCreatureBest.TabIndex = 10;
            //
            // lbGraphCaption
            //
            lbGraphCaption.AutoSize = true;
            lbGraphCaption.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lbGraphCaption.Location = new System.Drawing.Point(410, 82);
            lbGraphCaption.Name = "lbGraphCaption";
            lbGraphCaption.Size = new System.Drawing.Size(192, 15);
            lbGraphCaption.TabIndex = 11;
            lbGraphCaption.Text = "Possible wild-level outcomes";
            //
            // offspringPossibilities1
            //
            offspringPossibilities1.Location = new System.Drawing.Point(410, 102);
            offspringPossibilities1.Name = "offspringPossibilities1";
            offspringPossibilities1.Size = new System.Drawing.Size(430, 250);
            offspringPossibilities1.TabIndex = 12;
            //
            // lbMutationProbability
            //
            lbMutationProbability.AutoSize = true;
            lbMutationProbability.Location = new System.Drawing.Point(410, 360);
            lbMutationProbability.Name = "lbMutationProbability";
            lbMutationProbability.Size = new System.Drawing.Size(0, 15);
            lbMutationProbability.TabIndex = 13;
            //
            // BreedingSimulator
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(lbMutationProbability);
            Controls.Add(offspringPossibilities1);
            Controls.Add(lbGraphCaption);
            Controls.Add(pedigreeCreatureBest);
            Controls.Add(lbCaptionBest);
            Controls.Add(pedigreeCreatureFather);
            Controls.Add(lbCaptionFather);
            Controls.Add(pedigreeCreatureMother);
            Controls.Add(lbCaptionMother);
            Controls.Add(lbHint);
            Controls.Add(cbFather);
            Controls.Add(lbFatherLabel);
            Controls.Add(cbMother);
            Controls.Add(lbMotherLabel);
            Name = "BreedingSimulator";
            Size = new System.Drawing.Size(900, 420);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lbMotherLabel;
        private System.Windows.Forms.ComboBox cbMother;
        private System.Windows.Forms.Label lbFatherLabel;
        private System.Windows.Forms.ComboBox cbFather;
        private System.Windows.Forms.Label lbHint;
        private System.Windows.Forms.Label lbCaptionMother;
        private ARKBreedingStats.Pedigree.PedigreeCreature pedigreeCreatureMother;
        private System.Windows.Forms.Label lbCaptionFather;
        private ARKBreedingStats.Pedigree.PedigreeCreature pedigreeCreatureFather;
        private System.Windows.Forms.Label lbCaptionBest;
        private ARKBreedingStats.Pedigree.PedigreeCreature pedigreeCreatureBest;
        private System.Windows.Forms.Label lbGraphCaption;
        private ARKBreedingStats.OffspringPossibilities offspringPossibilities1;
        private System.Windows.Forms.Label lbMutationProbability;
    }
}
