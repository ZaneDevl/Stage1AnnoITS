﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Fatturazione.Models;

[Table("ba_docume_m005")]
[Index("Dotipfat", Name = "ba_docume_m00510")]
[Index("Doserddr", Name = "ba_docume_m00511")]
[Index("Dodatreg", Name = "ba_docume_m0052")]
[Index("Donumpro", Name = "ba_docume_m0053")]
[Index("Docodsog", Name = "ba_docume_m0054")]
[Index("Dorifcon", Name = "ba_docume_m0055")]
[Index("Dostatra", Name = "ba_docume_m0056")]
[Index("Dofidcar", Name = "ba_docume_m0057")]
[Index("Docodfat", Name = "ba_docume_m0058")]
[Index("Docodcau", Name = "ba_docume_m0059")]
public partial class BaDocumeM005
{
    [Key]
    [Column("DOSERIAL")]
    [StringLength(10)]
    [Unicode(false)]
    public string Doserial { get; set; }

    [Column("DOCODESE")]
    [StringLength(5)]
    [Unicode(false)]
    public string Docodese { get; set; }

    [Column("DONUMPER")]
    public int? Donumper { get; set; }

    [Column("DOCODCAU")]
    [StringLength(10)]
    [Unicode(false)]
    public string Docodcau { get; set; }

    [Column("DOFLCICL")]
    [StringLength(3)]
    [Unicode(false)]
    public string Doflcicl { get; set; }

    [Column("DOTIPDOC")]
    [StringLength(3)]
    [Unicode(false)]
    public string Dotipdoc { get; set; }

    [Column("DONUMREG", TypeName = "decimal(10, 0)")]
    public decimal? Donumreg { get; set; }

    [Column("DOCAUCON")]
    [StringLength(10)]
    [Unicode(false)]
    public string Docaucon { get; set; }

    [Column("DOCAUMAG")]
    [StringLength(10)]
    [Unicode(false)]
    public string Docaumag { get; set; }

    [Column("DOCAUMA2")]
    [StringLength(10)]
    [Unicode(false)]
    public string Docauma2 { get; set; }

    [Column("DOTIPMAG")]
    public int? Dotipmag { get; set; }

    [Column("DOCODMAG")]
    [StringLength(10)]
    [Unicode(false)]
    public string Docodmag { get; set; }

    [Column("DOCODMA2")]
    [StringLength(10)]
    [Unicode(false)]
    public string Docodma2 { get; set; }

    [Column("DOUNISTO")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dounisto { get; set; }

    [Column("DOUNIST2")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dounist2 { get; set; }

    [Column("DOCODAZI")]
    [StringLength(15)]
    [Unicode(false)]
    public string Docodazi { get; set; }

    [Column("DOCODUFF")]
    [StringLength(10)]
    [Unicode(false)]
    public string Docoduff { get; set; }

    [Column("DODATREG")]
    public LocalDate? Dodatreg { get; set; }

    [Column("DODATDOC")]
    public LocalDate? Dodatdoc { get; set; }

    [Column("DODATOPE")]
    public LocalDate? Dodatope { get; set; }

    [Column("DONUMDOC")]
    [StringLength(25)]
    [Unicode(false)]
    public string Donumdoc { get; set; }

    [Column("DOCODUTE")]
    [StringLength(15)]
    [Unicode(false)]
    public string Docodute { get; set; }

    [Column("DONUMIVA")]
    public int? Donumiva { get; set; }

    [Column("DOTIPIVA")]
    [StringLength(3)]
    [Unicode(false)]
    public string Dotipiva { get; set; }

    [Column("DOATTIVA")]
    [StringLength(5)]
    [Unicode(false)]
    public string Doattiva { get; set; }

    [Column("DONUMIV2")]
    public int? Donumiv2 { get; set; }

    [Column("DOTIPIV2")]
    [StringLength(3)]
    [Unicode(false)]
    public string Dotipiv2 { get; set; }

    [Column("DOANNPRO")]
    [StringLength(5)]
    [Unicode(false)]
    public string Doannpro { get; set; }

    [Column("DOSERPRO")]
    [StringLength(3)]
    [Unicode(false)]
    public string Doserpro { get; set; }

    [Column("DONUMPRO", TypeName = "decimal(10, 0)")]
    public decimal? Donumpro { get; set; }

    [Column("DOALFPRO")]
    [StringLength(5)]
    [Unicode(false)]
    public string Doalfpro { get; set; }

    [Column("DOCODVAL")]
    [StringLength(3)]
    [Unicode(false)]
    public string Docodval { get; set; }

    [Column("DOCAOVAL", TypeName = "decimal(12, 6)")]
    public decimal? Docaoval { get; set; }

    [Column("DOSTATUS")]
    public int? Dostatus { get; set; }

    [Column("DOFLPROV")]
    public int? Doflprov { get; set; }

    [Column("DOAGGPRO")]
    public int? Doaggpro { get; set; }

    [Column("DOTIPSOG")]
    [StringLength(3)]
    [Unicode(false)]
    public string Dotipsog { get; set; }

    [Column("DOCODSOG")]
    [StringLength(16)]
    [Unicode(false)]
    public string Docodsog { get; set; }

    [Column("DOTIPFAT")]
    [StringLength(3)]
    [Unicode(false)]
    public string Dotipfat { get; set; }

    [Column("DOSOGFAT")]
    [StringLength(16)]
    [Unicode(false)]
    public string Dosogfat { get; set; }

    [Column("DODIPFAT")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dodipfat { get; set; }

    [Column("DOCODFAT")]
    [StringLength(16)]
    [Unicode(false)]
    public string Docodfat { get; set; }

    [Column("DOANAFAT")]
    [StringLength(15)]
    [Unicode(false)]
    public string Doanafat { get; set; }

    [Column("DODIPSOG")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dodipsog { get; set; }

    [Column("DORIFENT")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dorifent { get; set; }

    [Column("DOUTIENT")]
    public int? Doutient { get; set; }

    [Column("DOIVAESE")]
    [StringLength(10)]
    [Unicode(false)]
    public string Doivaese { get; set; }

    [Column("DOFLSCOR")]
    public int? Doflscor { get; set; }

    [Column("DOANACON")]
    [StringLength(15)]
    [Unicode(false)]
    public string Doanacon { get; set; }

    [Column("DODIPCON")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dodipcon { get; set; }

    [Column("DOLINPRO")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dolinpro { get; set; }

    [Column("DOTIPAGE")]
    [StringLength(3)]
    [Unicode(false)]
    public string Dotipage { get; set; }

    [Column("DOCODAGE")]
    [StringLength(16)]
    [Unicode(false)]
    public string Docodage { get; set; }

    [Column("DOTIPSPE")]
    [StringLength(3)]
    [Unicode(false)]
    public string Dotipspe { get; set; }

    [Column("DOCODSPZ")]
    [StringLength(16)]
    [Unicode(false)]
    public string Docodspz { get; set; }

    [Column("DORUOAG1")]
    [StringLength(3)]
    [Unicode(false)]
    public string Doruoag1 { get; set; }

    [Column("DOAGESUP")]
    [StringLength(16)]
    [Unicode(false)]
    public string Doagesup { get; set; }

    [Column("DORUOAG2")]
    [StringLength(3)]
    [Unicode(false)]
    public string Doruoag2 { get; set; }

    [Column("DOCODPAG")]
    [StringLength(5)]
    [Unicode(false)]
    public string Docodpag { get; set; }

    [Column("DODATPAG")]
    public LocalDate? Dodatpag { get; set; }

    [Column("DOGIOFIS")]
    public int? Dogiofis { get; set; }

    [Column("DOSCOCAS", TypeName = "decimal(6, 2)")]
    public decimal? Doscocas { get; set; }

    [Column("DOVALCAS", TypeName = "decimal(20, 5)")]
    public decimal? Dovalcas { get; set; }

    [Column("DOFZSCFI")]
    public int? Dofzscfi { get; set; }

    [Column("DOFLIVAM")]
    public int? Doflivam { get; set; }

    [Column("DOFLRATE")]
    public int? Doflrate { get; set; }

    [Column("DOFLDCOR")]
    public int? Dofldcor { get; set; }

    [Column("DOTIPVET")]
    [StringLength(3)]
    [Unicode(false)]
    public string Dotipvet { get; set; }

    [Column("DOCODVET")]
    [StringLength(16)]
    [Unicode(false)]
    public string Docodvet { get; set; }

    [Column("DOCODVE2")]
    [StringLength(16)]
    [Unicode(false)]
    public string Docodve2 { get; set; }

    [Column("DOCODVE3")]
    [StringLength(16)]
    [Unicode(false)]
    public string Docodve3 { get; set; }

    [Column("DOCODPOR")]
    [StringLength(3)]
    [Unicode(false)]
    public string Docodpor { get; set; }

    [Column("DOCODSCA")]
    [StringLength(10)]
    [Unicode(false)]
    public string Docodsca { get; set; }

    [Column("DONOMPOR")]
    [StringLength(40)]
    [Unicode(false)]
    public string Donompor { get; set; }

    [Column("DOMODTRA")]
    [StringLength(10)]
    [Unicode(false)]
    public string Domodtra { get; set; }

    [Column("DODESTRA")]
    [StringLength(40)]
    [Unicode(false)]
    public string Dodestra { get; set; }

    [Column("DOCODASP")]
    [StringLength(10)]
    [Unicode(false)]
    public string Docodasp { get; set; }

    [Column("DODESASP")]
    [StringLength(40)]
    [Unicode(false)]
    public string Dodesasp { get; set; }

    [Column("DONUMCOL", TypeName = "decimal(15, 3)")]
    public decimal? Donumcol { get; set; }

    [Column("DONUMPAL", TypeName = "decimal(15, 3)")]
    public decimal? Donumpal { get; set; }

    [Column("DOPESLOR", TypeName = "decimal(20, 8)")]
    public decimal? Dopeslor { get; set; }

    [Column("DOPESNET", TypeName = "decimal(20, 8)")]
    public decimal? Dopesnet { get; set; }

    [Column("DOVOLUME", TypeName = "decimal(15, 3)")]
    public decimal? Dovolume { get; set; }

    [Column("DODATTRA")]
    public LocalDate? Dodattra { get; set; }

    [Column("DOORATRA")]
    [StringLength(2)]
    [Unicode(false)]
    public string Dooratra { get; set; }

    [Column("DOMINTRA")]
    [StringLength(2)]
    [Unicode(false)]
    public string Domintra { get; set; }

    [Column("DODATCON")]
    public LocalDate? Dodatcon { get; set; }

    [Column("DOORACON")]
    [StringLength(2)]
    [Unicode(false)]
    public string Dooracon { get; set; }

    [Column("DOMINCON")]
    [StringLength(2)]
    [Unicode(false)]
    public string Domincon { get; set; }

    [Column("DOSCOCO1", TypeName = "decimal(6, 2)")]
    public decimal? Doscoco1 { get; set; }

    [Column("DOSCOCO2", TypeName = "decimal(6, 2)")]
    public decimal? Doscoco2 { get; set; }

    [Column("DOSCOPAG", TypeName = "decimal(6, 2)")]
    public decimal? Doscopag { get; set; }

    [Column("DOVALCOM", TypeName = "decimal(20, 5)")]
    public decimal? Dovalcom { get; set; }

    [Column("DOVALSPA", TypeName = "decimal(20, 5)")]
    public decimal? Dovalspa { get; set; }

    [Column("DOFZSCCO")]
    public int? Dofzscco { get; set; }

    [Column("DOFZSPAG")]
    public int? Dofzspag { get; set; }

    [Column("DOIMPTRA", TypeName = "decimal(20, 5)")]
    public decimal? Doimptra { get; set; }

    [Column("DOIMPINC", TypeName = "decimal(20, 5)")]
    public decimal? Doimpinc { get; set; }

    [Column("DOIMPIMB", TypeName = "decimal(20, 5)")]
    public decimal? Doimpimb { get; set; }

    [Column("DOIMPSPE", TypeName = "decimal(20, 5)")]
    public decimal? Doimpspe { get; set; }

    [Column("DOIMPBOL", TypeName = "decimal(20, 5)")]
    public decimal? Doimpbol { get; set; }

    [Column("DOIMPARR", TypeName = "decimal(20, 5)")]
    public decimal? Doimparr { get; set; }

    [Column("DOTOTIMP", TypeName = "decimal(20, 5)")]
    public decimal? Dototimp { get; set; }

    [Column("DOTOTIVA", TypeName = "decimal(20, 5)")]
    public decimal? Dototiva { get; set; }

    [Column("DOBUSUNI")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dobusuni { get; set; }

    [Column("DOBANNOS")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dobannos { get; set; }

    [Column("DOBANCLF")]
    public int? Dobanclf { get; set; }

    [Column("DOCODLPZ")]
    [StringLength(20)]
    [Unicode(false)]
    public string Docodlpz { get; set; }

    [Column("DOCODLSC")]
    [StringLength(20)]
    [Unicode(false)]
    public string Docodlsc { get; set; }

    [Column("DOFLGPRO")]
    public int? Doflgpro { get; set; }

    [Column("DOFLTRAS")]
    public int? Dofltras { get; set; }

    [Column("DOFLCONT")]
    public int? Doflcont { get; set; }

    [Column("DORIFCON")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dorifcon { get; set; }

    [Column("DONOTCOR")]
    [StringLength(10)]
    [Unicode(false)]
    public string Donotcor { get; set; }

    [Column("DOFLGIOM")]
    public int? Doflgiom { get; set; }

    [Column("DOMOTCRE")]
    [StringLength(3)]
    [Unicode(false)]
    public string Domotcre { get; set; }

    [Column("DODATPLA")]
    public LocalDate? Dodatpla { get; set; }

    [Column("DOSERDIB")]
    [StringLength(10)]
    [Unicode(false)]
    public string Doserdib { get; set; }

    [Column("DOROWDIB")]
    public int? Dorowdib { get; set; }

    [Column("DOTIPPAL")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dotippal { get; set; }

    [Column("DOTIPCOL")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dotipcol { get; set; }

    [Column("DOTCONTA")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dotconta { get; set; }

    [Column("DOCOMMAR")]
    [StringLength(10)]
    [Unicode(false)]
    public string Docommar { get; set; }

    [Column("DOPARSPE")]
    [StringLength(10)]
    [Unicode(false)]
    public string Doparspe { get; set; }

    [Column("DOFORRIP")]
    public int? Doforrip { get; set; }

    [Column("DODATIVA")]
    public LocalDate? Dodativa { get; set; }

    [Column("DODATRET")]
    public LocalDate? Dodatret { get; set; }

    [Column("DOSTOREID")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dostoreid { get; set; }

    [Column("DOCCRPRE")]
    [StringLength(10)]
    [Unicode(false)]
    public string Doccrpre { get; set; }

    [Column("DOIMPPRE", TypeName = "decimal(20, 5)")]
    public decimal? Doimppre { get; set; }

    [Column("DOPAGCON")]
    public int? Dopagcon { get; set; }

    [Column("DOFLINTR")]
    public int? Doflintr { get; set; }

    [Column("DOSTATRA")]
    public int? Dostatra { get; set; }

    [Column("DOCODTRA")]
    [StringLength(10)]
    [Unicode(false)]
    public string Docodtra { get; set; }

    [Column("DOPARCHF")]
    public int? Doparchf { get; set; }

    [Column("DOFLCCOM")]
    public int? Doflccom { get; set; }

    [Column("DOFLRIVA")]
    public int? Doflriva { get; set; }

    [Column("DOCODLIN")]
    [StringLength(3)]
    [Unicode(false)]
    public string Docodlin { get; set; }

    [Column("DOIMPASS", TypeName = "decimal(20, 5)")]
    public decimal? Doimpass { get; set; }

    [Column("DOFLCOLL")]
    public int? Doflcoll { get; set; }

    [Column("DOPARMSP")]
    [StringLength(10)]
    [Unicode(false)]
    public string Doparmsp { get; set; }

    [Column("DOROWMSP")]
    public int? Dorowmsp { get; set; }

    [Column("DOFIDCAR")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dofidcar { get; set; }

    [Column("DOFILGEN")]
    public int? Dofilgen { get; set; }

    [Column("DOFLEVPA")]
    public int? Doflevpa { get; set; }

    [Column("DOPRIORI")]
    public int? Dopriori { get; set; }

    [Column("DOPARPRE")]
    [StringLength(10)]
    [Unicode(false)]
    public string Doparpre { get; set; }

    [Column("DOFZPREV")]
    public int? Dofzprev { get; set; }

    [Column("DOFLPICK")]
    public int? Doflpick { get; set; }

    [Column("DOCOMMAG")]
    [StringLength(10)]
    [Unicode(false)]
    public string Docommag { get; set; }

    [Column("DOOFFCRM")]
    public int? Dooffcrm { get; set; }

    [Column("DODIBKEY")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dodibkey { get; set; }

    [Column("DODIBROW")]
    public int? Dodibrow { get; set; }

    [Column("DOPKLNOC")]
    public int? Dopklnoc { get; set; }

    [Column("DOCOUPON")]
    [StringLength(50)]
    [Unicode(false)]
    public string Docoupon { get; set; }

    [Column("DOSALCOUPON")]
    [StringLength(50)]
    [Unicode(false)]
    public string Dosalcoupon { get; set; }

    [Column("DOCOUPSIGN")]
    [StringLength(1)]
    [Unicode(false)]
    public string Docoupsign { get; set; }

    [Column("DOCOUPQTA")]
    public int? Docoupqta { get; set; }

    [Column("DOFLCCID")]
    public int? Doflccid { get; set; }

    [Column("DOAPPINC")]
    public int? Doappinc { get; set; }

    [Column("DOIMPMIN", TypeName = "decimal(20, 5)")]
    public decimal? Doimpmin { get; set; }

    [Column("DOVALMIN")]
    [StringLength(3)]
    [Unicode(false)]
    public string Dovalmin { get; set; }

    [Column("DOPERINC", TypeName = "decimal(6, 2)")]
    public decimal? Doperinc { get; set; }

    [Column("DOVALINC", TypeName = "decimal(20, 5)")]
    public decimal? Dovalinc { get; set; }

    [Column("DOFLSCOC")]
    public int? Doflscoc { get; set; }

    [Column("DOFLSINC")]
    public int? Doflsinc { get; set; }

    [Column("DOSCCART")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dosccart { get; set; }

    [Column("DOINCART")]
    [StringLength(10)]
    [Unicode(false)]
    public string Doincart { get; set; }

    [Column("DOFLPRVA")]
    public int? Doflprva { get; set; }

    [Column("DODISLOC", TypeName = "decimal(20, 8)")]
    public decimal? Dodisloc { get; set; }

    [Column("DOCONTRA")]
    [StringLength(30)]
    [Unicode(false)]
    public string Docontra { get; set; }

    [Column("DOIMPABB", TypeName = "decimal(20, 5)")]
    public decimal? Doimpabb { get; set; }

    [Column("DOTIPTER")]
    [StringLength(3)]
    [Unicode(false)]
    public string Dotipter { get; set; }

    [Column("DOCODTER")]
    [StringLength(16)]
    [Unicode(false)]
    public string Docodter { get; set; }

    [Column("DOTIPDIP")]
    [StringLength(3)]
    [Unicode(false)]
    public string Dotipdip { get; set; }

    [Column("DOCODDIP")]
    [StringLength(16)]
    [Unicode(false)]
    public string Docoddip { get; set; }

    [Column("DOCODREP")]
    [StringLength(3)]
    [Unicode(false)]
    public string Docodrep { get; set; }

    [Column("DONUMRAT")]
    public int? Donumrat { get; set; }

    [Column("DOVALRAT", TypeName = "decimal(20, 5)")]
    public decimal? Dovalrat { get; set; }

    [Column("DOTIPDEP")]
    public int? Dotipdep { get; set; }

    [Column("DOSERDDR")]
    [StringLength(10)]
    [Unicode(false)]
    public string Doserddr { get; set; }

    [Column("DOROWDDR")]
    public int? Dorowddr { get; set; }

    [Column("DOQTAWARN")]
    public int? Doqtawarn { get; set; }

    [Column("DOFLGINT")]
    public int? Doflgint { get; set; }

    [Column("DOCONPRE")]
    public int? Doconpre { get; set; }

    [Column("DOOLDPRE", TypeName = "decimal(20, 5)")]
    public decimal? Dooldpre { get; set; }

    [Column("DOTRACKING")]
    [StringLength(50)]
    [Unicode(false)]
    public string Dotracking { get; set; }

    [Column("DOORDEXP")]
    public LocalDate? Doordexp { get; set; }

    [Column("DODDTRIE")]
    [StringLength(10)]
    [Unicode(false)]
    public string Doddtrie { get; set; }

    [Column("DOFLSCOP")]
    public int? Doflscop { get; set; }

    [Column("DOSCPART")]
    [StringLength(10)]
    [Unicode(false)]
    public string Doscpart { get; set; }

    [Column("DOCHKDIB")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dochkdib { get; set; }

    [Column("DOCHKOCL")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dochkocl { get; set; }

    [Column("DOPOSCNT")]
    [StringLength(10)]
    [Unicode(false)]
    public string Doposcnt { get; set; }

    [Column("DOFLASPE")]
    public int? Doflaspe { get; set; }

    [Column("DORIFSCN")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dorifscn { get; set; }

    [Column("DOPESRSP", TypeName = "decimal(20, 8)")]
    public decimal? Dopesrsp { get; set; }

    [Column("DOVALRSP", TypeName = "decimal(20, 5)")]
    public decimal? Dovalrsp { get; set; }

    [Column("DOFASEDOC")]
    [StringLength(5)]
    [Unicode(false)]
    public string Dofasedoc { get; set; }

    [Column("DOSEREVT")]
    [StringLength(10)]
    [Unicode(false)]
    public string Doserevt { get; set; }

    [Column("DOTMSTAMP", TypeName = "datetime")]
    public Instant? Dotmstamp { get; set; }

    [Column("DOCRECOMP")]
    [StringLength(15)]
    [Unicode(false)]
    public string Docrecomp { get; set; }

    [Column("DOPRZNET")]
    public int? Doprznet { get; set; }

    [Column("DOFLSTFA")]
    public int? Doflstfa { get; set; }

    [Column("DOIDPLANM")]
    [StringLength(10)]
    [Unicode(false)]
    public string Doidplanm { get; set; }

    [Column("DOTOTVALCON", TypeName = "decimal(20, 5)")]
    public decimal? Dototvalcon { get; set; }

    [Column("DOSTOREPOS")]
    [StringLength(10)]
    [Unicode(false)]
    public string Dostorepos { get; set; }

    [Column("DOFLGCALPRO")]
    public int? Doflgcalpro { get; set; }

    [Column("cpupdtms", TypeName = "datetime")]
    public Instant? Cpupdtms { get; set; }

    [Column("cpccchk")]
    [StringLength(10)]
    [Unicode(false)]
    public string Cpccchk { get; set; }

    [Column("DOLCKGENSCA")]
    public int? Dolckgensca { get; set; }

    [Column("DOSTORBOL", TypeName = "decimal(20, 5)")]
    public decimal? Dostorbol { get; set; }

    [InverseProperty("DoserialNavigation")]
    public virtual ICollection<BaDocume005> BaDocume005s { get; set; } = new List<BaDocume005>();
}