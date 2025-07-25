using FluentValidation;
using AgizDisSagligiApp.Core.DTOs;
using AgizDisSagligiApp.Core.Enums;

namespace AgizDisSagligiApp.Application.Validators
{
    public class CreateActivityDtoValidator : AbstractValidator<CreateActivityDto>
    {
        public CreateActivityDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Aktivite adı gereklidir")
                .MaximumLength(200).WithMessage("Aktivite adı en fazla 200 karakter olabilir");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Geçerli bir aktivite türü seçiniz");

            RuleFor(x => x.CompletedAt)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Tamamlanma tarihi gelecekte olamaz");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Süre 0'dan büyük olmalıdır")
                .LessThanOrEqualTo(86400).WithMessage("Süre 24 saatten fazla olamaz"); // 24 hours in seconds

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).WithMessage("Notlar en fazla 2000 karakter olabilir");

            RuleFor(x => x.Tags)
                .MaximumLength(500).WithMessage("Etiketler en fazla 500 karakter olabilir");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).When(x => x.Rating.HasValue)
                .WithMessage("Puan 1-5 arasında olmalıdır");
        }
    }

    public class UpdateActivityDtoValidator : AbstractValidator<UpdateActivityDto>
    {
        public UpdateActivityDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Aktivite adı gereklidir")
                .MaximumLength(200).WithMessage("Aktivite adı en fazla 200 karakter olabilir");

            RuleFor(x => x.CompletedAt)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Tamamlanma tarihi gelecekte olamaz");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Süre 0'dan büyük olmalıdır")
                .LessThanOrEqualTo(86400).WithMessage("Süre 24 saatten fazla olamaz");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).WithMessage("Notlar en fazla 2000 karakter olabilir");

            RuleFor(x => x.Tags)
                .MaximumLength(500).WithMessage("Etiketler en fazla 500 karakter olabilir");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).When(x => x.Rating.HasValue)
                .WithMessage("Puan 1-5 arasında olmalıdır");
        }
    }

    public class CreateGoalDtoValidator : AbstractValidator<CreateGoalDto>
    {
        public CreateGoalDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Hedef başlığı gereklidir")
                .MaximumLength(200).WithMessage("Hedef başlığı en fazla 200 karakter olabilir");

            RuleFor(x => x.ActivityType)
                .IsInEnum().WithMessage("Geçerli bir aktivite türü seçiniz");

            RuleFor(x => x.TargetCount)
                .GreaterThan(0).WithMessage("Hedef sayısı 0'dan büyük olmalıdır")
                .LessThanOrEqualTo(1000).WithMessage("Hedef sayısı 1000'den fazla olamaz");

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(DateTime.UtcNow.AddDays(30))
                .WithMessage("Başlangıç tarihi en fazla 30 gün sonra olabilir");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).WithMessage("Bitiş tarihi başlangıç tarihinden sonra olmalıdır")
                .LessThanOrEqualTo(DateTime.UtcNow.AddYears(1))
                .WithMessage("Bitiş tarihi en fazla 1 yıl sonra olabilir");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Geçerli bir öncelik seviyesi seçiniz");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir");

            RuleFor(x => x.Tags)
                .MaximumLength(500).WithMessage("Etiketler en fazla 500 karakter olabilir");

            RuleFor(x => x.RecurrencePattern)
                .MaximumLength(100).WithMessage("Tekrar deseni en fazla 100 karakter olabilir");

            RuleFor(x => x.RemindBefore)
                .GreaterThanOrEqualTo(0).When(x => x.RemindBefore.HasValue)
                .WithMessage("Hatırlatma süresi negatif olamaz")
                .LessThanOrEqualTo(10080).When(x => x.RemindBefore.HasValue)
                .WithMessage("Hatırlatma süresi en fazla 1 hafta (10080 dakika) olabilir");

            RuleFor(x => x.Reward)
                .MaximumLength(500).WithMessage("Ödül en fazla 500 karakter olabilir");

            RuleFor(x => x.RewardPoints)
                .GreaterThanOrEqualTo(0).When(x => x.RewardPoints.HasValue)
                .WithMessage("Ödül puanı negatif olamaz")
                .LessThanOrEqualTo(10000).When(x => x.RewardPoints.HasValue)
                .WithMessage("Ödül puanı en fazla 10000 olabilir");
        }
    }

    public class UpdateGoalDtoValidator : AbstractValidator<UpdateGoalDto>
    {
        public UpdateGoalDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Hedef başlığı gereklidir")
                .MaximumLength(200).WithMessage("Hedef başlığı en fazla 200 karakter olabilir");

            RuleFor(x => x.TargetCount)
                .GreaterThan(0).WithMessage("Hedef sayısı 0'dan büyük olmalıdır")
                .LessThanOrEqualTo(1000).WithMessage("Hedef sayısı 1000'den fazla olamaz");

            RuleFor(x => x.EndDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Bitiş tarihi gelecekte olmalıdır")
                .LessThanOrEqualTo(DateTime.UtcNow.AddYears(1))
                .WithMessage("Bitiş tarihi en fazla 1 yıl sonra olabilir");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Geçerli bir hedef durumu seçiniz");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Geçerli bir öncelik seviyesi seçiniz");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir");

            RuleFor(x => x.Tags)
                .MaximumLength(500).WithMessage("Etiketler en fazla 500 karakter olabilir");

            RuleFor(x => x.RemindBefore)
                .GreaterThanOrEqualTo(0).When(x => x.RemindBefore.HasValue)
                .WithMessage("Hatırlatma süresi negatif olamaz")
                .LessThanOrEqualTo(10080).When(x => x.RemindBefore.HasValue)
                .WithMessage("Hatırlatma süresi en fazla 1 hafta (10080 dakika) olabilir");

            RuleFor(x => x.Reward)
                .MaximumLength(500).WithMessage("Ödül en fazla 500 karakter olabilir");

            RuleFor(x => x.RewardPoints)
                .GreaterThanOrEqualTo(0).When(x => x.RewardPoints.HasValue)
                .WithMessage("Ödül puanı negatif olamaz")
                .LessThanOrEqualTo(10000).When(x => x.RewardPoints.HasValue)
                .WithMessage("Ödül puanı en fazla 10000 olabilir");
        }
    }
}