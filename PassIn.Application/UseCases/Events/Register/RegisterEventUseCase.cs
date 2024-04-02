using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.Register {
    public class RegisterEventUseCase {
        public ResponseRegisteredEventJson Execute(RequestEventJson request) {
            Validate(request);

            var dbContext = new PassInDbContext();
            var entity = new Infrastructure.Entities.Event { // todo: use automapper
                Title = request.Title,
                Details = request.Details,
                Maximum_Attendees = request.MaximumAttendees,
                Slug = request.Title.ToLower().Replace(" ", "-")
            };

            dbContext.Events.Add(entity); // todo: change to AddAsync and use Task
            dbContext.SaveChanges();
            return new ResponseRegisteredEventJson {
                Id = entity.Id
            };
        }

        private void Validate(RequestEventJson request) { // todo: Use FluentValidation
            if (request.MaximumAttendees <= 0) throw new ErrorOnValidationException("The Maximum attendees is invalid.");
            if (string.IsNullOrWhiteSpace(request.Title)) throw new ErrorOnValidationException("The Title is required.");
            if (string.IsNullOrWhiteSpace(request.Details)) throw new ErrorOnValidationException("The Title is required.");
        }
    }
}
